using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investment.AHP.Domain.Documents.Crm;

public class DocumentsCrmContext : IDocumentsCrmContext
{
    private readonly ICrmService _crmService;

    public DocumentsCrmContext(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task<string> GetDocumentLocation(AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var request = new invln_getahpapplicationdocumentlocationRequest { invln_applicationid = applicationId.Value };
        return await _crmService.ExecuteAsync<invln_getahpapplicationdocumentlocationRequest, invln_getahpapplicationdocumentlocationResponse>(
            request,
            x => x.invln_documentlocation,
            cancellationToken);
    }
}
