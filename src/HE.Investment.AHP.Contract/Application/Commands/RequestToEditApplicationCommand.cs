using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record RequestToEditApplicationCommand(AhpApplicationId Id, string? RequestToEditReason) : IRequest<OperationResult>;
