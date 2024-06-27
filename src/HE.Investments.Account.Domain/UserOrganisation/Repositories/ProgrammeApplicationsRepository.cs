using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProgrammeApplicationsRepository : IProgrammeApplicationsRepository
{
    private readonly ICrmService _crmService;

    public ProgrammeApplicationsRepository(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task<IList<Contract.UserOrganisation.Programme>> GetAllProgrammes(ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        return
        [
            new(ProgrammeType.Loans, await GetLoansApplications(userAccount, cancellationToken)),
            new(ProgrammeType.Ahp, await GetAhpProjects(userAccount, cancellationToken)),
        ];
    }

    public async Task<IList<Contract.UserOrganisation.Programme>> GetLoanProgrammes(UserAccount userAccount, CancellationToken cancellationToken)
    {
        return
        [
            new(ProgrammeType.Loans, await GetLoansApplications(userAccount, cancellationToken)),
        ];
    }

    public async Task<bool> HasAnyAhpApplication(ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var ahpProjects = await GetAhpProjects(userAccount, cancellationToken);

        return ahpProjects.Any();
    }

    private async Task<IList<UserAppliance>> GetLoansApplications(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var req = new invln_getloanapplicationsforaccountandcontactRequest
        {
            invln_accountid = userAccount.SelectedOrganisationId().ToGuidAsString(),
            invln_externalcontactid = userAccount.UserGlobalId.ToString(),
            invln_usehetables = "true",
        };

        var loanApplications = (await _crmService.ExecuteAsync
            <invln_getloanapplicationsforaccountandcontactRequest,
                invln_getloanapplicationsforaccountandcontactResponse,
                IList<LoanApplicationDto>>(req, r => r.invln_loanapplications, cancellationToken))
            .OrderByDescending(application => application.createdOn ?? DateTime.MinValue)
            .ThenByDescending(x => x.LastModificationOn);

        return loanApplications.Select(a => new UserAppliance(
                HeApplianceId.From(a.loanApplicationId),
                a.ApplicationName,
                LoanApplicationStatusMapper.MapToPortalStatus(a.loanApplicationExternalStatus)))
            .ToList();
    }

    private async Task<IList<UserAppliance>> GetAhpProjects(ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getahpprojectsRequest
        {
            invln_userid = userAccount.UserGlobalId.ToString(),
            invln_accountid = userAccount.SelectedOrganisationId().ToGuidAsString(),
            invln_consortiumid = userAccount.Consortium.GetConsortiumIdAsString()?.ToGuidAsString(),
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 1000 }),
        };

        var projects = await _crmService.ExecuteAsync<invln_getahpprojectsRequest, invln_getahpprojectsResponse, PagedResponseDto<AhpProjectDto>>(
            request,
            r => r.invln_listOfAhpProjects,
            cancellationToken);

        return projects.items.Select(a => new UserAppliance(
                HeApplianceId.From(a.FrontDoorProjectId),
                a.AhpProjectName))
            .ToList();
    }
}
