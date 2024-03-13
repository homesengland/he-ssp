using HE.Investments.Account.Shared;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideExpectedStartDateCommandHandler : ProjectBaseCommandHandler<ProvideExpectedStartDateCommand>
{
    public ProvideExpectedStartDateCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideExpectedStartDateCommand request)
    {
        project.ProvideExpectedStartDate(new ExpectedStartDate(request.ExpectedStartDate?.Month, request.ExpectedStartDate?.Year));
    }
}
