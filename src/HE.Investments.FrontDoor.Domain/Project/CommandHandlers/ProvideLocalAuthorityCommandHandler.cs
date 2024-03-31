extern alias Org;

using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using ProjectLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideLocalAuthorityCommandHandler : ProjectBaseCommandHandler<ProvideLocalAuthorityCommand>
{
    public ProvideLocalAuthorityCommandHandler(IProjectRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideLocalAuthorityCommand request)
    {
        project.ProvideLocalAuthority(ProjectLocalAuthority.New(request.LocalAuthorityCode, request.LocalAuthorityName));
    }
}
