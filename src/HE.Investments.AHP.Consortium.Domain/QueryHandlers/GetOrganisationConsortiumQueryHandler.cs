using System.Collections.ObjectModel;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetOrganisationConsortiumQueryHandler : IRequestHandler<GetOrganisationConsortiumQuery, ConsortiumBasicDetails?>
{
    private readonly IConsortiumRepository _repository;

    public GetOrganisationConsortiumQueryHandler(IConsortiumRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsortiumBasicDetails?> Handle(GetOrganisationConsortiumQuery request, CancellationToken cancellationToken)
    {
        var consortia = await _repository.GetConsortiumsListByMemberId(request.OrganisationId, cancellationToken);
        var consortium = consortia.FirstOrDefault(x => x.ProgrammeId == request.ProgrammeId);

        return consortium.IsProvided()
            ? new ConsortiumBasicDetails(
                consortium!.Id,
                consortium.ProgrammeId,
                consortium.LeadPartner.Id,
                new ReadOnlyCollection<OrganisationId>(consortium.ActiveMembers.Select(x => x.Id).ToList()))
            : null;
    }
}
