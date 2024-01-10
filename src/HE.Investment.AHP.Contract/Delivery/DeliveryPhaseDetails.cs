using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseDetails(
    string ApplicationName,
    string Id,
    string Name,
    TypeOfHomes? TypeOfHomes,
    BuildActivityTypeForNewBuild? BuildActivityTypeForNewBuild,
    BuildActivityTypeForRehab? BuildActivityTypeForRehab,
    int? NumberOfHomes,
    bool IsUnregisteredBody,
    DateDetails? AcquisitionDate,
    DateDetails? AcquisitionPaymentDate,
    DateDetails? StartOnSiteDate,
    DateDetails? StartOnSitePaymentDate,
    DateDetails? PracticalCompletionDate,
    DateDetails? PracticalCompletionPaymentDate,
    bool? IsAdditionalPaymentRequested);
