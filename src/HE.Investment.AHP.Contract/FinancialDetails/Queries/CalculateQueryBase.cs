using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record CalculateQueryBase(Guid ApplicationId) : IRequest<(OperationResult OperationResult, CalculationResult CalculationResult)>;
