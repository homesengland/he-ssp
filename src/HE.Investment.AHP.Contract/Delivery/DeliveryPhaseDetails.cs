using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseDetails(
    string ApplicationName,
    string Id,
    string Name,
    SectionStatus Status,
    TypeOfHomes? TypeOfHomes = null,
    BuildActivityType? BuildActivityType = null,
    IList<BuildActivityType>? AvailableBuildActivityTypes = null,
    bool IsReconfiguringExistingNeeded = false,
    bool? ReconfiguringExisting = null,
    int? NumberOfHomes = null,
    DeliveryPhaseTranchesDto? Tranches = null,
    bool IsUnregisteredBody = false,
    bool IsOnlyCompletionMilestone = false,
    DateDetails? AcquisitionDate = null,
    DateDetails? AcquisitionPaymentDate = null,
    DateDetails? StartOnSiteDate = null,
    DateDetails? StartOnSitePaymentDate = null,
    DateDetails? PracticalCompletionDate = null,
    DateDetails? PracticalCompletionPaymentDate = null,
    bool? IsAdditionalPaymentRequested = null);
