using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record HoldApplicationCommand(AhpApplicationId Id, string? HoldReason) : IRequest<OperationResult>, IUpdateApplicationCommand;
