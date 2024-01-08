using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseDetails(
    string ApplicationName,
    string Id,
    string Name,
    TypeOfHomes? TypeOfHomes = null,
    int? NumberOfHomes = null,
    string? Acquisition = null,
    string? StartOnSite = null,
    string? PracticalCompletion = null);
