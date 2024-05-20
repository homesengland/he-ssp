using HE.Investments.AHP.Consortium.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class GetAvailableProgrammesQueryHandler : IRequestHandler<GetAvailableProgrammesQuery, AvailableProgramme[]>
{
    public Task<AvailableProgramme[]> Handle(GetAvailableProgrammesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new[]
        {
            new AvailableProgramme(
                "d5fe3baa-eeae-ee11-a569-0022480041cf",
                "Affordable Homes Programme 21-26 Continuous Market Engagement",
                "Affordable Homes Programme 2021-2026 Continuous Market Engagement",
                "AHP 21-26 CME"),
        });
    }
}
