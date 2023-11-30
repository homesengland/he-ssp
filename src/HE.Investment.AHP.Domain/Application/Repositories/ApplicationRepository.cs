using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Data;
using HE.Investments.Account.Shared;
using HE.Investments.Common.CRM;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IApplicationCrmContext _applicationCrmContext;
    private readonly IAccountUserContext _accountUserContext;

    public ApplicationRepository(IApplicationCrmContext applicationCrmContext, IAccountUserContext accountUserContext)
    {
        _applicationCrmContext = applicationCrmContext;
        _accountUserContext = accountUserContext;
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

    public async Task<IList<ApplicationEntity>> GetAll(CancellationToken cancellationToken)
    {
        var applications = await _applicationCrmContext.GetAll(CrmFields.ApplicationToRead, cancellationToken);

        return applications.Select(CreateEntity).ToList();
    }

    public async Task<ApplicationEntity> Save(ApplicationEntity application, CancellationToken cancellationToken)
    {
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
                SectionStatusMapper.ToDomain(application.schemeInformationSectionCompletionStatus),
                SectionStatusMapper.ToDomain(application.homeTypesSectionCompletionStatus),
                SectionStatusMapper.ToDomain(application.financialDetailsSectionCompletionStatus),
                SectionStatusMapper.ToDomain(application.deliveryPhasesSectionCompletionStatus)));
    }
}
