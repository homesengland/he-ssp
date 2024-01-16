using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record ProvideApplicationStatusCommand(AhpApplicationId Id, ApplicationStatus Status, string? ChangeStatusReason)
    : IRequest<OperationResult>, IUpdateApplicationCommand;
