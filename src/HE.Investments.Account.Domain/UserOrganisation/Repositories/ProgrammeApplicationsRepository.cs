using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProgrammeApplicationsRepository : IProgrammeApplicationsRepository
{
    private readonly ICrmService _crmService;

    private readonly IFeatureManager _featureManager;

    public ProgrammeApplicationsRepository(ICrmService crmService, IFeatureManager featureManager)
    {
        _crmService = crmService;
        _featureManager = featureManager;
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

    public async Task<bool> HasAnyAhpApplication(UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AhpProgram, cancellationToken))
        {
            return false;
        }

        var request = new invln_getmultipleahpapplicationsRequest
        {
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            inlvn_userid = string.Empty,
            invln_appfieldstoretrieve = nameof(invln_scheme.invln_schemename),
        };

        var applications = await _crmService.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);

        return applications.Any();
    }

    private async Task<IList<UserApplication>> GetLoansApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
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
            invln_appfieldstoretrieve = $"{nameof(invln_scheme.invln_schemename)},{nameof(invln_scheme.StatusCode)},{nameof(invln_ExternalStatus)}".ToLowerInvariant(),
        };

        var applications = await _crmService.ExecuteAsync<invln_getmultipleahpapplicationsRequest, invln_getmultipleahpapplicationsResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_ahpapplications,
            cancellationToken);

        return applications.Select(a => new UserApplication(
                HeApplicationId.From(a.id),
                a.name,
                AhpApplicationStatusMapper.MapToPortalStatus(a.applicationStatus)))
            .ToList();
    }
}
