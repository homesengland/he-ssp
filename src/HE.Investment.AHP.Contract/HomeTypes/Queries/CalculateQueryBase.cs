using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateQueryBase(string ApplicationId, string HomeTypeId) : IRequest<(OperationResult OperationResult, CalculationResult CalculationResult)>;
