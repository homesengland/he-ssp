using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.Storage.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class UpdateFrontDoorDecisionCommandHandler : ProjectBaseCommandHandler<UpdateFrontDoorDecisionCommand>
{
    public UpdateFrontDoorDecisionCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, UpdateFrontDoorDecisionCommand request)
    {
        var mapper = new ApplicationTypeMapper();
        var decision = mapper.ToDto(request.ApplicationType);
        project.UpdateFrontDoorDecision(decision);
    }
}
