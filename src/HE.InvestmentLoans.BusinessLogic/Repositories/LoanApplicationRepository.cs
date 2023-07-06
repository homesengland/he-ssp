using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.InvestmentLoans.CRM.Model;
using Microsoft.Xrm.Sdk;
using System.Text.Json;

namespace HE.InvestmentLoans.BusinessLogic.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly IOrganizationService _serviceClient;

    public LoanApplicationRepository(IOrganizationService serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public void Save(LoanApplicationDto loanApplicationDto)
    {
        string loanApplicationSerialized = JsonSerializer.Serialize(loanApplicationDto);
        var req = new invln_sendinvestmentloansdatatocrmRequest  //Name of Custom API
        {
            invln_entityfieldsparameters = loanApplicationSerialized  //Input Parameter
        };

        _serviceClient.Execute(req);
    }
}
