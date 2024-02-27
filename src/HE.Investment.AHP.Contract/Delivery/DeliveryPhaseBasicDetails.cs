using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseBasicDetails(
    string ApplicationName,
    string Id,
    string? Name,
    int? NumberOfHomes,
    DateDetails? Acquisition,
    DateDetails? StartOnSite,
    DateDetails? PracticalCompletion);
