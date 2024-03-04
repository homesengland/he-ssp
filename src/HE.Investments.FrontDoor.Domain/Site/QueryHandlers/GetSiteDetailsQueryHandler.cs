using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetails>
{
    public Task<SiteDetails> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SiteDetails
        {
            Id = request.SiteId,
            Name = "Test site",
            ProjectName = "Test name",
        });
    }
}
