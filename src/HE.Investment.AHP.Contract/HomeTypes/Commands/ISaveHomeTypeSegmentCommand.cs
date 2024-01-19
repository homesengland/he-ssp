using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public interface ISaveHomeTypeSegmentCommand : IRequest<OperationResult>
{
    public AhpApplicationId ApplicationId { get; }

    public HomeTypeId HomeTypeId { get; }
}
