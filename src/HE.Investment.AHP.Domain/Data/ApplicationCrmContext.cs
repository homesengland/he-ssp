using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.Common.CrmCommunication.Serialization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Account.Shared;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Data;

public class ApplicationCrmContext : IApplicationCrmContext
{
    private readonly ICrmService _service;
    private readonly IAccountUserContext _accountUserContext;

    public ApplicationCrmContext(ICrmService service, IAccountUserContext accountUserContext)
    {
        _service = service;
        _accountUserContext = accountUserContext;
    }

    public async Task<AhpApplicationDto> GetById(string id, IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getahpapplicationRequest()
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_applicationid = id,
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };

        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("AhpApplication", id);
        }

        return response.First();
    }

    public async Task<bool> IsExist(string applicationName, CancellationToken cancellationToken)
    {
        var dto = new AhpApplicationDto
        {
            name = applicationName,
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

    public async Task<IList<AhpApplicationDto>> GetAll(IList<string> fieldsToRetrieve, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_appfieldstoretrieve = FormatFields(fieldsToRetrieve),
        };

        return await _service.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);
    }

    public async Task<string> Save(AhpApplicationDto dto, IList<string> fieldsToUpdate, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var request = new invln_setahpapplicationRequest
        {
            invln_userid = account.UserGlobalId.ToString(),
            invln_organisationid = account.AccountId.ToString(),
            invln_application = CrmResponseSerializer.Serialize(dto),
            invln_fieldstoupdate = FormatFields(fieldsToUpdate),
        };

        return await _service.ExecuteAsync<invln_setahpapplicationRequest, invln_setahpapplicationResponse>(
            request,
            r => r.invln_applicationid,
            cancellationToken);
    }

    private static string FormatFields(IList<string> fieldsToRetrieve)
    {
        return string.Join(",", fieldsToRetrieve.Select(f => f.ToLowerInvariant()));
    }
}
