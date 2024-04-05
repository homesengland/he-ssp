using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialCheckAnswersQuery(AhpApplicationId ApplicationId) : IRequest<GetFinancialCheckAnswersResult>;

public record GetFinancialCheckAnswersResult(string ApplicationName, ApplicationStatus ApplicationStatus, SectionStatus SectionStatus, LandValueSummary LandValue, TotalSchemeCost TotalSchemeCost, TotalContributions TotalContributions);

public record LandValueSummary(decimal? PurchasePrice, decimal? CurrentValue, YesNoType IsPublicLand);

public record TotalSchemeCost(decimal? CurrentValue, decimal? WorkCosts, decimal? OnCosts, decimal? Total);

public record TotalContributions(decimal? SchemaFunding, decimal? YourContributions, decimal? GrantsFromOtherPublicBodies, decimal? Total);
