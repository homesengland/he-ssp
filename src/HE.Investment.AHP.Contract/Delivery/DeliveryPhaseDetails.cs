namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseDetails(
    string ApplicationName,
    string Id,
    string Name,
    int? NumberOfHomes,
    string? Acquisition,
    string? StartOnSite,
    string? PracticalCompletion);
