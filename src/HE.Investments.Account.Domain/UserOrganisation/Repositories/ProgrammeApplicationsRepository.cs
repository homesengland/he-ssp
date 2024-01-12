using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProgrammeApplicationsRepository : IProgrammeApplicationsRepository
{
    private readonly ICrmService _crmService;

    public ProgrammeApplicationsRepository(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task<IList<Programme>> GetAllProgrammes(UserAccount userAccount, CancellationToken cancellationToken)
    {
        return new List<Programme>
        {
            new(ProgrammeType.Loans, await GetLoansApplications(userAccount, cancellationToken)),
            new(ProgrammeType.Ahp, await GetAhpApplications(userAccount, cancellationToken)),
        };
    }

    public async Task<IList<Programme>> GetLoanProgrammes(UserAccount userAccount, CancellationToken cancellationToken)
    {
        return new List<Programme>
        {
            new(ProgrammeType.Loans, await GetLoansApplications(userAccount, cancellationToken)),
        };
    }

    private async Task<IList<UserApplication>> GetLoansApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest()
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
        };

        var loanApplications = (await _crmService.ExecuteAsync
            <invln_getloanapplicationsforaccountandcontactRequest,
                invln_getloanapplicationsforaccountandcontactResponse,
                IList<LoanApplicationDto>>(req, r => r.invln_loanapplications, cancellationToken))
            .OrderByDescending(application => application.createdOn ?? DateTime.MinValue)
            .ThenByDescending(x => x.LastModificationOn);

        return loanApplications.Select(a => new UserApplication(
                HeApplicationId.From(a.loanApplicationId),
                a.ApplicationName,
                ApplicationStatusMapper.MapToPortalStatus(a.loanApplicationExternalStatus)))
            .ToList();
    }

    private async Task<IList<UserApplication>> GetAhpApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getmultipleahpapplicationsRequest
        {
            inlvn_userid = userAccount.CanViewAllApplications() ? string.Empty : userAccount.UserGlobalId.ToString(),
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            invln_appfieldstoretrieve = $"{nameof(invln_scheme.invln_schemename)},{nameof(invln_scheme.statuscode)}".ToLowerInvariant(),
        };

        var applications = await _crmService.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);

        return applications.Select(a => new UserApplication(
                HeApplicationId.From(a.id),
                a.name,
                ApplicationStatusMapper.MapToPortalStatus(a.applicationStatus)))
            .ToList();
    }
}
