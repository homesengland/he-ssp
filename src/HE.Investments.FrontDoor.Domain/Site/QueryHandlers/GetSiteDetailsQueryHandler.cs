using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetails>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetSiteDetailsQueryHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<SiteDetails> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProject(request.ProjectId, await _accountUserContext.GetSelectedAccount(), cancellationToken);

        return new SiteDetails
        {
            Id = request.SiteId,
            Name = "Test site",
            ProjectName = project.Name.Value,
        };
    }
}
