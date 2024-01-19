using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateQueryBase(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IRequest<(OperationResult OperationResult, CalculationResult CalculationResult)>;
