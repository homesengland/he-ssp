using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public interface IDeliveryCommand : IRequest<OperationResult>
{
    public string ApplicationId { get; }
}
