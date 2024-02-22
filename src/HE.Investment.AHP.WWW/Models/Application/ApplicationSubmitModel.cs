namespace HE.Investment.AHP.WWW.Models.Application;

public record ApplicationSubmitModel(
    string ApplicationId,
    string ApplicationName,
    string ReferenceNumber,
    string SiteName,
    string Tenure,
    string NumberOfHomes,
    string FundingRequested,
    string TotalSchemeCost,
    string? RepresentationsAndWarranties);
