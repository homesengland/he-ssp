using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideRegionCommandHandler : ProjectBaseCommandHandler<ProvideRegionCommand>
{
    public ProvideRegionCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideRegionCommand request)
    {
        project.ProvideRegions(new Regions(request.Regions));
    }
}
