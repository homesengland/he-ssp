using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumsListQueryHandler : IRequestHandler<GetConsortiumsListQuery, ConsortiumsList>
{
    private readonly IConsortiumRepository _repository;

    private readonly IMediator _mediator;

    private readonly IAccountUserContext _accountUserContext;

    public GetConsortiumsListQueryHandler(
        IConsortiumRepository repository,
        IMediator mediator,
        IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _mediator = mediator;
        _accountUserContext = accountUserContext;
    }

    public async Task<ConsortiumsList> Handle(GetConsortiumsListQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisation = account.SelectedOrganisation();
        var ahpProgrammes = await _mediator.Send(new GetProgrammesQuery(ProgrammeType.Ahp), cancellationToken);
        var consortiumsList = await _repository.GetConsortiumsListByMemberId(organisation.OrganisationId, cancellationToken);

        return new ConsortiumsList(CreateConsortiumByMemberRole(consortiumsList, ahpProgrammes, organisation), organisation.RegisteredCompanyName);
    }

    private static List<ConsortiumByMemberRole> CreateConsortiumByMemberRole(
        IList<ConsortiumEntity> consortiumsList,
        IList<Programme.Contract.Programme> ahpProgrammes,
        OrganisationBasicInfo organisation)
    {
        return consortiumsList.Select(x => new ConsortiumByMemberRole(
            x.Id,
            ahpProgrammes.Single(y => y.Id == x.ProgrammeId),
            x.LeadPartner.OrganisationName,
            x.LeadPartner.Id == organisation.OrganisationId ? ConsortiumMembershipRole.LeadPartner : ConsortiumMembershipRole.Member)).ToList();
    }
}
