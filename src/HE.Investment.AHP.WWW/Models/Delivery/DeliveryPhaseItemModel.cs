namespace HE.Investment.AHP.WWW.Models.Delivery;

public record DeliveryPhaseItemModel(
    string DeliveryPhaseId,
    string? DeliveryPhaseName,
    int? NumberOfHomes,
    string? Acquisition,
    string? StartOnSite,
    string? PracticalCompletion);
