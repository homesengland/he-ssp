using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public interface IDeliveryCommand : IRequest<OperationResult>
{
    public string ApplicationId { get; }
}
