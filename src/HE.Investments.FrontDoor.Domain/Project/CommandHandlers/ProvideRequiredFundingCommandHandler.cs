using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideRequiredFundingCommandHandler : ProjectBaseCommandHandler<ProvideRequiredFundingCommand>
{
    public ProvideRequiredFundingCommandHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
        : base(projectRepository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideRequiredFundingCommand request)
    {
        project.ProvideRequiredFunding(new RequiredFunding(request.RequiredFundingOption));
    }
}
