using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record CheckAnswersCommand(AhpApplicationId Id) : IRequest<OperationResult>;
