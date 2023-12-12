using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Events;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investments.Account.Shared;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Utils.Pagination;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;
    private readonly IAccountUserContext _accountUserContext;
    private readonly IEventDispatcher _eventDispatcher;

    public ApplicationRepository(IApplicationCrmContext applicationCrmContext, IAccountUserContext accountUserContext, IEventDispatcher eventDispatcher)
    {
        _applicationCrmContext = applicationCrmContext;
        _accountUserContext = accountUserContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        var application = await _applicationCrmContext.GetById(id.Value, CrmFields.ApplicationToRead, cancellationToken);

        return CreateEntity(application);
    }

    public async Task<bool> IsExist(ApplicationName applicationName, CancellationToken cancellationToken)
    {
        return await _applicationCrmContext.IsExist(applicationName.Name, cancellationToken);
    }

    public async Task<ApplicationBasicInfo> GetApplicationBasicInfo(ApplicationId id, CancellationToken cancellationToken)
    {
        var application = await GetById(id, cancellationToken);
        return new ApplicationBasicInfo(application.Id, application.Name, application.Tenure?.Value ?? Tenure.Undefined, ApplicationStatus.Draft);
    }

    public async Task<PaginationResult<ApplicationWithFundingDetails>> GetApplicationsWithFundingDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var applications = await _applicationCrmContext.GetAll(CrmFields.ApplicationListToRead, cancellationToken);

        var filtered = applications
            .OrderByDescending(x => x.lastExternalModificationOn)
            .Skip(paginationRequest.ItemsPerPage * (paginationRequest.Page - 1))
            .Take(paginationRequest.ItemsPerPage)
            .Select(x => new ApplicationWithFundingDetails(
                new ApplicationId(x.id),
                x.name,
                ApplicationStatusMapper.MapToPortalStatus(x.applicationStatus),
                x.noOfHomes,
                x.fundingRequested))
            .ToList();

        return new PaginationResult<ApplicationWithFundingDetails>(filtered, paginationRequest.Page, paginationRequest.ItemsPerPage, applications.Count);
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken)
    {
        if (application is { IsModified: false, IsNew: false })
        {
            return application;
        }

        var account = await _accountUserContext.GetSelectedAccount();
        var dto = new AhpApplicationDto
        {
            id = application.Id.IsEmpty() ? null : application.Id.Value,
            name = application.Name.Name,
            tenure = ApplicationTenureMapper.ToDto(application.Tenure),
            organisationId = account.AccountId.ToString(),
        };

        var id = await _applicationCrmContext.Save(dto, CrmFields.ApplicationToUpdate, cancellationToken);
        if (application.Id.IsEmpty())
        {
            application.SetId(new ApplicationId(id));

            await _eventDispatcher.Publish(new ApplicationHasBeenCreatedEvent(id), cancellationToken);
        }

        return application;
    }

    private static ApplicationEntity CreateEntity(AhpApplicationDto application)
    {
        return new ApplicationEntity(
            new ApplicationId(application.id),
            new ApplicationName(application.name ?? "Unknown"),
            ApplicationStatusMapper.MapToPortalStatus(application.applicationStatus),
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
