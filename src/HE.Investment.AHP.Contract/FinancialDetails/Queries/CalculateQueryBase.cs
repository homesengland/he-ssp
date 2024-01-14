using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record CalculateQueryBase(AhpApplicationId ApplicationId) : IRequest<(OperationResult OperationResult, CalculationResult CalculationResult)>;
