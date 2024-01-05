using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseDetails(
    string ApplicationName,
    string Id,
    string Name,
    int? NumberOfHomes,
    DateDetails? AcquisitionDate,
    DateDetails? AcquisitionPaymentDate,
    DateDetails? StartOnSiteDate,
    DateDetails? StartOnSitePaymentDate,
    DateDetails? PracticalCompletionDate,
    DateDetails? PracticalCompletionPaymentDate);
