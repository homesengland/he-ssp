using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.BusinessLogic.Files;

public class FileApplicationRepository : IFileApplicationRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public FileApplicationRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task<string> GetBaseFilePath(LoanApplicationId applicationId, CancellationToken cancellationToken)
    {
        var req = new invln_getfilelocationforapplicationloanRequest
        {
            invln_loanapplicationid = applicationId.ToString(),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getfilelocationforapplicationloanResponse
                       ?? throw new NotFoundException(nameof(CompanyStructureEntity), applicationId.ToString());
        return response.invln_filelocation;
    }
}
