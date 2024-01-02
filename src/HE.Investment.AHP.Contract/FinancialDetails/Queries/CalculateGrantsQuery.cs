namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record CalculateGrantsQuery(
    Guid ApplicationId,
    string? CountyCouncilGrants,
    string? DhscExtraCareGrants,
    string? LocalAuthorityGrants,
    string? SocialServicesGrants,
    string? HealthRelatedGrants,
    string? LotteryGrants,
    string? OtherPublicBodiesGrants) : CalculateQueryBase(ApplicationId);
