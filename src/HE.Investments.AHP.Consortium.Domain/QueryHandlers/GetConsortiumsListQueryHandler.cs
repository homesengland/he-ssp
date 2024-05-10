using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumsListQueryHandler : IRequestHandler<GetConsortiumsListQuery, ConsortiumsList>
{
    private readonly IConsortiumRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IAhpAccessContext _ahpAccessContext;

    public GetConsortiumsListQueryHandler(
        IConsortiumRepository repository,
        IAccountUserContext accountUserContext,
        IAhpAccessContext ahpAccessContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _ahpAccessContext = ahpAccessContext;
    }

    public async Task<ConsortiumsList> Handle(GetConsortiumsListQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisation = account.SelectedOrganisation();

        var consortiumsList = await _repository.GetConsortiumsListByMemberId(organisation.OrganisationId, cancellationToken);

        return new ConsortiumsList(CreateConsortiumByMemberRole(consortiumsList, organisation), organisation.RegisteredCompanyName, await _ahpAccessContext.CanManageConsortium());
    }

    private static List<ConsortiumByMemberRole> CreateConsortiumByMemberRole(IList<ConsortiumEntity> consortiumsList, OrganisationBasicInfo organisation)
    {
        return consortiumsList.Select(x => new ConsortiumByMemberRole(
            x.Id,
            x.Programme,
            x.LeadPartner.OrganisationName,
            x.LeadPartner.Id == organisation.OrganisationId ? ConsortiumMembershipRole.LeadPartner : ConsortiumMembershipRole.Member)).ToList();
    }
}
