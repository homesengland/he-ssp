using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.Commands;

public abstract record DeliveryCommandBase(string ApplicationId) : IRequest<OperationResult>;
