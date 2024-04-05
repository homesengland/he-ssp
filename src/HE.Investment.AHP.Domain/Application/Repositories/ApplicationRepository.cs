using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Mappers;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
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

    private readonly IAhpProgrammeRepository _programmeRepository;

    private readonly IEventDispatcher _eventDispatcher;

    public ApplicationRepository(IApplicationCrmContext applicationCrmContext, IEventDispatcher eventDispatcher, IAhpProgrammeRepository programmeRepository)
    {
        _applicationCrmContext = applicationCrmContext;
        _eventDispatcher = eventDispatcher;
        _programmeRepository = programmeRepository;
    }

    public async Task<ApplicationEntity> GetById(AhpApplicationId id, UserAccount userAccount, CancellationToken cancellationToken, bool fetchPreviousStatus = false)
    {
        var application = await GetAhpApplicationDto(id, userAccount, CrmFields.ApplicationToRead.ToList(), cancellationToken);

        // TODO: fetch previous application status when implemented on CRM side and flag is set to true
        return CreateEntity(application, userAccount, null);
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
        return new ApplicationBasicInfo(
            application.Id,
            application.SiteId,
            application.Name,
            application.Tenure.Value,
            application.Status,
            await _programmeRepository.GetProgramme(id, cancellationToken),
            new ApplicationStateFactory(userAccount));
    }

    public async Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(
        UserAccount userAccount,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken)
    {
        return await GetApplications(userAccount, paginationRequest, null, cancellationToken);
    }

    public async Task<ApplicationWithFundingDetails> GetApplicationWithFundingDetailsById(
        AhpApplicationId id,
        UserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var application = await GetAhpApplicationDto(id, userAccount, CrmFields.ApplicationWithFundingDetailsToRead.ToList(), cancellationToken);

        return CreateApplicationWithFundingDetails(application);
    }

    public async Task<PaginationResult<ApplicationWithFundingDetails>> GetSiteApplications(SiteId siteId, UserAccount userAccount, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        return await GetApplications(userAccount, paginationRequest, a => a.siteId == siteId.Value, cancellationToken);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (application.IsStatusModified)
        {
            await _applicationCrmContext.ChangeApplicationStatus(
                application.Id.Value,
                organisationId.Value,
                application.Status,
                application.ChangeStatusReason,
                cancellationToken);
        }

        if (application is { IsModified: false, IsNew: false })
        {
            return application;
        }

        var dto = new AhpApplicationDto
        {
            id = application.Id.IsNew ? null : application.Id.Value,
            name = application.Name.Name,
            tenure = ApplicationTenureMapper.ToDto(application.Tenure),
            organisationId = organisationId.Value.ToString(),
            applicationStatus = AhpApplicationStatusMapper.MapToCrmStatus(application.Status),
            siteId = application.SiteId.Value,
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

    public async Task DispatchEvents(DomainEntity domainEntity, CancellationToken cancellationToken)
    {
        await _eventDispatcher.Publish(domainEntity, cancellationToken);
    }

    private static ApplicationEntity CreateEntity(AhpApplicationDto application, UserAccount userAccount, ApplicationStatus? previousStatus)
    {
        var applicationStatus = AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus);

        return new ApplicationEntity(
            new SiteId(application.siteId),
            new AhpApplicationId(application.id),
            new ApplicationName(application.name ?? "Unknown"),
            applicationStatus,
            ApplicationTenureMapper.ToDomain(application.tenure)!,
            new ApplicationStateFactory(userAccount, previousStatus),
            new ApplicationReferenceNumber(application.referenceNumber),
            new ApplicationSections(
                new List<ApplicationSection>
                {
                    new(SectionType.Scheme, SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus, applicationStatus)),
                    new(SectionType.HomeTypes, SectionStatusMapper.ToDomain(application.homeTypesSectionCompletionStatus, applicationStatus)),
                    new(SectionType.FinancialDetails, SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus, applicationStatus)),
                    new(SectionType.DeliveryPhases, SectionStatusMapper.ToDomain(application.deliveryPhasesSectionCompletionStatus, applicationStatus)),
                }),
            new AuditEntry(
                application.lastExternalModificationBy?.firstName,
                application.lastExternalModificationBy?.lastName,
                application.lastExternalModificationOn),
            application.dateSubmitted.IsProvided() ? new AuditEntry(application.lastExternalModificationBy?.firstName, application.lastExternalModificationBy?.lastName, application.dateSubmitted) : null); // TODO: AB#63432 Fetch submit user when added to CRM endpoint
    }

    private async Task<PaginationResult<ApplicationWithFundingDetails>> GetApplications(
        UserAccount userAccount,
        PaginationRequest paginationRequest,
        Predicate<AhpApplicationDto>? filter,
        CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        var applications = userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplications(organisationId, CrmFields.ApplicationListToRead.ToList(), cancellationToken)
            : await _applicationCrmContext.GetUserApplications(organisationId, CrmFields.ApplicationListToRead.ToList(), cancellationToken);

        var filtered = applications
            .Where(x => filter == null || filter(x))
            .OrderByDescending(x => x.lastExternalModificationOn)
            .TakePage(paginationRequest)
            .Select(CreateApplicationWithFundingDetails)
            .ToList();

        return new PaginationResult<ApplicationWithFundingDetails>(filtered, paginationRequest.Page, paginationRequest.ItemsPerPage, filtered.Count);
    }

    private ApplicationWithFundingDetails CreateApplicationWithFundingDetails(AhpApplicationDto ahpApplicationDto)
    {
        var otherApplicationCosts = OtherApplicationCostsMapper.MapToOtherApplicationCosts(ahpApplicationDto);

        return new ApplicationWithFundingDetails(
            new SiteId(ahpApplicationDto.siteId),
            new AhpApplicationId(ahpApplicationDto.id),
            ahpApplicationDto.name,
            ahpApplicationDto.referenceNumber,
            AhpApplicationStatusMapper.MapToPortalStatus(ahpApplicationDto.applicationStatus),
            ApplicationTenureMapper.ToDomain(ahpApplicationDto.tenure)!.Value,
            ahpApplicationDto.noOfHomes,
            ahpApplicationDto.fundingRequested,
            otherApplicationCosts.ExpectedTotalCosts(),
            ahpApplicationDto.currentLandValue,
            ahpApplicationDto.dateSubmitted.IsProvided() ? true : null); // TODO: task AB#91399 fetch value from crm
    }

    private async Task<AhpApplicationDto> GetAhpApplicationDto(AhpApplicationId id, UserAccount userAccount, IList<string> crmFields, CancellationToken cancellationToken)
    {
        var organisationId = userAccount.SelectedOrganisationId().Value;
        return userAccount.CanViewAllApplications()
            ? await _applicationCrmContext.GetOrganisationApplicationById(id.Value, organisationId, crmFields, cancellationToken)
            : await _applicationCrmContext.GetUserApplicationById(id.Value, organisationId, crmFields, cancellationToken);
    }
}
