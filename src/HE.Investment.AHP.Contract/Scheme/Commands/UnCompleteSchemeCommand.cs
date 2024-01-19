using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record UnCompleteSchemeCommand(AhpApplicationId ApplicationId) : IRequest<OperationResult>, IUpdateSchemeCommand;
