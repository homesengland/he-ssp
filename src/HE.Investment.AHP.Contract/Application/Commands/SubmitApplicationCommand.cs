using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record SubmitApplicationCommand(AhpApplicationId Id) : IRequest<OperationResult>;
