using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetConsortiumDetailsQueryHandler : IRequestHandler<GetConsortiumDetailsQuery, ConsortiumDetails>
{
    public Task<ConsortiumDetails> Handle(GetConsortiumDetailsQuery request, CancellationToken cancellationToken)
    {
        var result = new ConsortiumDetails(
            request.ConsortiumId,
            "Affordable Homes Programme 21-26 Continuous Market Engagement",
            new OrganisationDetails("Cactus Developments", "Tree street", "Westminster", "W1 9GG", "654321", null),
            new[]
            {
                new OrganisationDetails("Cactus Developments", "Tree street", "Westminster", "W1 9GG", "654321", null),
                new OrganisationDetails("Homes limited", "Ivy Road", "Hammersmith", "W17 9HH", "123456", null),
            });

        return Task.FromResult(result);
    }
}
