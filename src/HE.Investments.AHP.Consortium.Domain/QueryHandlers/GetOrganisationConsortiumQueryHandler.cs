using System.Collections.ObjectModel;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetOrganisationConsortiumQueryHandler(IConsortiumRepository repository)
    : IRequestHandler<GetOrganisationConsortiumQuery, ConsortiumBasicDetails?>
{
    public async Task<ConsortiumBasicDetails?> Handle(GetOrganisationConsortiumQuery request, CancellationToken cancellationToken)
    {
        var consortia = await repository.GetConsortiumsListByMemberId(request.OrganisationId, cancellationToken);
        var consortium = consortia.FirstOrDefault(x => x.Programme.Id == request.ProgrammeId);

        return consortium.IsProvided()
            ? new ConsortiumBasicDetails(
                consortium!.Id,
                consortium.Programme.Id,
                consortium.LeadPartner.Id,
                new ReadOnlyCollection<OrganisationId>(consortium.ActiveMembers.Select(x => x.Id).ToList()))
            : null;
    }
}
