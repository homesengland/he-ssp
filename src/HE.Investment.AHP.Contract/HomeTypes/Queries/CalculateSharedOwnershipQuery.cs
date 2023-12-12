using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record CalculateSharedOwnershipQuery(
    string ApplicationId,
    string HomeTypeId,
    string? MarketValue,
    string? InitialSalePercentage,
    string? SharedOwnershipWeeklyRent) : IRequest<OperationResult>;
