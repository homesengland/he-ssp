using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideHomesNumberCommandHandler : ProjectBaseCommandHandler<ProvideHomesNumberCommand>
{
    public ProvideHomesNumberCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideHomesNumberCommand request)
    {
        project.ProvideHomesNumber(new HomesNumber(request.HomesNumber));
    }
}
