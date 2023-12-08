using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateAffordableRentQuery(
    string ApplicationId,
    string HomeTypeId,
    string? HomeMarketValue,
    string? HomeWeeklyRent,
    string? AffordableWeeklyRent,
    YesNoType TargetRentExceedMarketRent) : IRequest<OperationResult>;
