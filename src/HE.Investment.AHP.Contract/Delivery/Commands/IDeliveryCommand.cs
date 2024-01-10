using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public interface IDeliveryCommand : IRequest<OperationResult>
{
    public AhpApplicationId ApplicationId { get; }
}
