namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record CalculateExpectedContributionsQuery(
        Guid ApplicationId,
        string? RentalIncomeBorrowing,
        string? SalesOfHomesOnThisScheme,
        string? SalesOfHomesOnOtherSchemes,
        string? OwnResources,
        string? RcgfContribution,
        string? OtherCapitalSources,
        string? SharedOwnershipSales,
        string? HomesTransferValue)
    : CalculateQueryBase(ApplicationId);
