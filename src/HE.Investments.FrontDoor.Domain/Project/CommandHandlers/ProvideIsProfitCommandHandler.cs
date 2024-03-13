using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideIsProfitCommandHandler : ProjectBaseCommandHandler<ProvideIsProfitCommand>
{
    public ProvideIsProfitCommandHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
        : base(projectRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideIsProfitCommand request)
    {
        project.ProvideIsProfit(new IsProfit(request.IsProfit));
    }
}
