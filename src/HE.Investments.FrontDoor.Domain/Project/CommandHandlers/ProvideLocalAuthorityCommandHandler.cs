using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : ProjectBaseCommandHandler<ProvideLocalAuthorityCommand>
{
    public ProvideLocalAuthorityCommandHandler(IProjectRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideLocalAuthorityCommand request)
    {
        project.ProvideLocalAuthority(request.LocalAuthorityId);
    }
}
