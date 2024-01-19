using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record WithdrawApplicationCommand(AhpApplicationId Id, string? WithdrawReason) : IRequest<OperationResult>, IUpdateApplicationCommand;
