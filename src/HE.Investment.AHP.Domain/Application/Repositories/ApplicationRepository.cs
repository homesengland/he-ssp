using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;

    private readonly IEventDispatcher _eventDispatcher;

    public ApplicationRepository(IApplicationCrmContext applicationCrmContext, IEventDispatcher eventDispatcher)
    {
        _applicationCrmContext = applicationCrmContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<ApplicationEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var application = userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplicationById(id.Value, organisationId, CrmFields.ApplicationToRead.ToList(), cancellationToken)
            : await _applicationCrmContext.GetUserApplicationById(id.Value, organisationId, CrmFields.ApplicationToRead.ToList(), cancellationToken);

        return CreateEntity(application);
    }

    public async Task<bool> IsNameExist(ApplicationName applicationName, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        return await _applicationCrmContext.IsNameExist(applicationName.Name, organisationId.Value, cancellationToken);
    }

    public async Task<bool> IsExist(AhpApplicationId applicationId, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var application = await _applicationCrmContext.GetUserApplicationById(applicationId.Value, organisationId.Value, CrmFields.ApplicationToRead.ToList(), cancellationToken);

        return application.IsProvided();
    }

    public async Task<ApplicationBasicInfo> GetApplicationBasicInfo(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var application = await GetById(id, userAccount, cancellationToken);
        return new ApplicationBasicInfo(application.Id, application.Name, application.Tenure?.Value ?? Tenure.Undefined, application.Status);
    }

    public async Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(
        UserAccount userAccount,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var applications = userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplications(organisationId, CrmFields.ApplicationListToRead.ToList(), cancellationToken)
            : await _applicationCrmContext.GetUserApplications(organisationId, CrmFields.ApplicationListToRead.ToList(), cancellationToken);

        var filtered = applications
            .OrderByDescending(x => x.lastExternalModificationOn)
            .TakePage(paginationRequest)
            .Select(x => new ApplicationWithFundingDetails(
                new AhpApplicationId(x.id),
                x.name,
                AhpApplicationStatusMapper.MapToPortalStatus(x.applicationStatus),
                ApplicationTenureMapper.ToDomain(x.tenure)!.Value,
                x.noOfHomes,
                x.fundingRequested))
            .ToList();

        return new PaginationResult<ApplicationWithFundingDetails>(filtered, paginationRequest.Page, paginationRequest.ItemsPerPage, applications.Count);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (application is { IsModified: false, IsNew: false })
        {
            return application;
        }

        var dto = new AhpApplicationDto
        {
            // TODO: AB#88650 Assign application to site
            id = application.Id.IsNew ? null : application.Id.Value,
            name = application.Name.Name,
            tenure = ApplicationTenureMapper.ToDto(application.Tenure),
            organisationId = organisationId.Value.ToString(),
            applicationStatus = ApplicationStatusMapper.MapToCrmStatus(application.Status),
        };

        var id = await _applicationCrmContext.Save(dto, organisationId.Value, CrmFields.ApplicationToUpdate.ToList(), cancellationToken);
        if (application.Id.IsNew)
        {
            var applicationId = AhpApplicationId.From(id);
            application.SetId(applicationId);

            await _eventDispatcher.Publish(new ApplicationHasBeenCreatedEvent(applicationId), cancellationToken);
        }

        return application;
    }

    public async Task Hold(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (application is { IsModified: false })
        {
            return;
        }

        var applicationId = new Guid(application.Id.Value);

        await _applicationCrmContext.ChangeApplicationStatus(
            applicationId,
            organisationId.Value,
            application.Status,
            application.HoldReason?.Value,
            cancellationToken);
    }

    public async Task Withdraw(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (application is { IsModified: false })
        {
            return;
        }

        var applicationId = new Guid(application.Id.Value);

        await _applicationCrmContext.ChangeApplicationStatus(
            applicationId,
            organisationId.Value,
            application.Status,
            application.WithdrawReason?.Value,
            cancellationToken);
    }

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }

    private static ApplicationEntity CreateEntity(AhpApplicationDto application)
    {
        return new ApplicationEntity(
            new SiteId("1"), // TODO: AB#88650 Assign application to site
            new AhpApplicationId(application.id),
            new ApplicationName(application.name ?? "Unknown"),
            AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus),
            new ApplicationReferenceNumber(application.referenceNumber),
            ApplicationTenureMapper.ToDomain(application.tenure),
            new AuditEntry(
                application.lastExternalModificationBy?.firstName,
                application.lastExternalModificationBy?.lastName,
                application.lastExternalModificationOn),
            new ApplicationSections(
                new List<ApplicationSection>
                {
                    new(SectionType.Scheme, SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus)),
                    new(SectionType.HomeTypes, SectionStatusMapper.ToDomain(application.homeTypesSectionCompletionStatus)),
                    new(SectionType.FinancialDetails, SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus)),
                    new(SectionType.DeliveryPhases, SectionStatusMapper.ToDomain(application.deliveryPhasesSectionCompletionStatus)),
                }));
    }
}
