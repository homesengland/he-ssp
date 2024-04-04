using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public interface IProvideProjectDetailsCommand : IRequest<OperationResult>
{
    public FrontDoorProjectId ProjectId { get; init; }
}
