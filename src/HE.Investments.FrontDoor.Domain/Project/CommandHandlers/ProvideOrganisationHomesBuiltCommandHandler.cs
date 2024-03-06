using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideOrganisationHomesBuiltCommandHandler : ProjectBaseCommandHandler<ProvideOrganisationHomesBuiltCommand>
{
    public ProvideOrganisationHomesBuiltCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideOrganisationHomesBuiltCommand request)
    {
        project.ProvideOrganisationHomesBuilt(new OrganisationHomesBuilt(request.OrganisationHomesBuilt));
    }
}
