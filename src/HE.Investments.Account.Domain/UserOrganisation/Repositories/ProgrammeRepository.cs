using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly ICrmService _crmService;

    public ProgrammeRepository(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task<IList<Programme>> GetAllProgrammes(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest()
        {
            invln_accountid = userAccount.AccountId.ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
        };

        var loanApplications = (await _crmService.ExecuteAsync
        <invln_getloanapplicationsforaccountandcontactRequest,
            invln_getloanapplicationsforaccountandcontactResponse,
            IList<LoanApplicationDto>>(req, r => r.invln_loanapplications, cancellationToken))
            .OrderByDescending(application => application.createdOn ?? DateTime.MinValue)
            .ThenByDescending(x => x.LastModificationOn);

        return new List<Programme>
        {
            new(
                ProgrammeType.Loans,
                loanApplications.Select(a => new UserApplication(
                        a.loanApplicationId,
                        a.name,
                        ApplicationStatusMapper.MapToPortalStatus(a.loanApplicationExternalStatus)))
                    .ToList()),
            new(ProgrammeType.Ahp, new List<UserApplication>()),
        };
    }
}
