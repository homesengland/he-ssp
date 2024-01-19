using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record CalculateGrantsQuery(
    AhpApplicationId ApplicationId,
    string? CountyCouncilGrants,
    string? DhscExtraCareGrants,
    string? LocalAuthorityGrants,
    string? SocialServicesGrants,
    string? HealthRelatedGrants,
    string? LotteryGrants,
    string? OtherPublicBodiesGrants) : CalculateQueryBase(ApplicationId);
