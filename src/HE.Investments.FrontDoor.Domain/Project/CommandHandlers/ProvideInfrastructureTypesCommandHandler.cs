using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideInfrastructureTypesCommandHandler : ProjectBaseCommandHandler<ProvideInfrastructureTypesCommand>
{
    public ProvideInfrastructureTypesCommandHandler(IProjectRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override void Perform(ProjectEntity project, ProvideInfrastructureTypesCommand request)
    {
        project.ProvideInfrastructureTypes(new ProjectInfrastructure(request.InfrastructureTypes));
    }
}
