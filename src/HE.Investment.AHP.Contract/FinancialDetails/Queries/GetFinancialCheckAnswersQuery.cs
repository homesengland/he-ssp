using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialCheckAnswersQuery(string ApplicationId) : IRequest<GetFinancialCheckAnswersResult>;

public record GetFinancialCheckAnswersResult(string ApplicationName, bool AreAllQuestionsAnswered, LandValueSummary LandValue, TotalSchemeCost TotalSchemeCost, TotalContributions TotalContributions);

public record LandValueSummary(decimal? PurchasePrice, decimal? CurrentValue, bool? IsPublicLand);

public record TotalSchemeCost(decimal? PurchasePrice, decimal? WorkCosts, decimal? OnCosts, decimal? Total);

public record TotalContributions(decimal? YourContributions, decimal? GrantsFromOtherPublicBodies, decimal? Total);
