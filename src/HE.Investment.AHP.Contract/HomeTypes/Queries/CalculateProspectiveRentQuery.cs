using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateProspectiveRentQuery(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? MarketRent,
    string? ProspectiveRent,
    YesNoType TargetRentExceedMarketRent) : IRequest<OperationResult>;
