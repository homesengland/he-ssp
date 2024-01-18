using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialCheckAnswersQuery(AhpApplicationId ApplicationId) : IRequest<GetFinancialCheckAnswersResult>;

public record GetFinancialCheckAnswersResult(string ApplicationName, SectionStatus SectionStatus, LandValueSummary LandValue, TotalSchemeCost TotalSchemeCost, TotalContributions TotalContributions);

public record LandValueSummary(decimal? PurchasePrice, decimal? CurrentValue, YesNoType IsPublicLand);

public record TotalSchemeCost(decimal? CurrentValue, decimal? WorkCosts, decimal? OnCosts, decimal? Total);

public record TotalContributions(decimal? YourContributions, decimal? GrantsFromOtherPublicBodies, decimal? Total);
