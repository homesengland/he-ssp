using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideAffordableHomesAmountCommandHandler : ProjectBaseCommandHandler<ProvideAffordableHomesAmountCommand>
{
    public ProvideAffordableHomesAmountCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideAffordableHomesAmountCommand request)
    {
        project.ProvideAffordableHomesAmount(new ProjectAffordableHomesAmount(request.AffordableHomesAmount));
    }
}
