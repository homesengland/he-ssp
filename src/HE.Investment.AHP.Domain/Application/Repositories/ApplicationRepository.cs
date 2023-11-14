using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Account.Shared;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ICrmService _service;
    private readonly IAccountUserContext _accountUserContext;

    public ApplicationRepository(ICrmService service, IAccountUserContext accountUserContext)
    {
        _service = service;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationEntity> GetById(ApplicationId id, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getahpapplicationRequest()
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = id.Value,
        };

        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("AhpApplication", id.Value);
        }

        return CreateEntity(response.First());
    }

    public async Task<bool> IsExist(ApplicationName applicationName, CancellationToken cancellationToken)
    {
        var dto = new AhpApplicationDto
        {
            name = applicationName.Name,
        };

        var request = new invln_checkifapplicationwithgivennameexistsRequest
        {
            invln_application = CrmResponseSerializer.Serialize(dto),
        };

        var response = await _service.ExecuteAsync<invln_checkifapplicationwithgivennameexistsRequest, invln_checkifapplicationwithgivennameexistsResponse>(
            request,
            r => r.invln_applicationexists,
            cancellationToken);

        return bool.TryParse(response, out var result) && result;
    }

    public async Task<ApplicationBasicInfo> GetApplicationBasicInfo(ApplicationId id, CancellationToken cancellationToken)
    {
        var application = await GetById(id, cancellationToken);
        return new ApplicationBasicInfo(application.Id, application.Tenure?.Value ?? Tenure.Undefined, ApplicationStatus.Draft);
    }

    public async Task<IList<ApplicationEntity>> GetAll(CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
        };

        var applications = await _service.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);

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

        var request = new invln_setahpapplicationRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_application = CrmResponseSerializer.Serialize(dto),
        };

        var id = await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            r => r.invln_applicationid,
            cancellationToken);

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
            ApplicationTenureMapper.ToDomain(application.tenure));
    }
}
