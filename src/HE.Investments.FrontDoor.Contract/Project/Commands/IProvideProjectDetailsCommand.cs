using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public interface IProvideProjectDetailsCommand : IRequest<OperationResult>
{
    public FrontDoorProjectId ProjectId { get; init; }
}
