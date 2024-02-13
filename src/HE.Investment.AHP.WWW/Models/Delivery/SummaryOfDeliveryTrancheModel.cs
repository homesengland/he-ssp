using HE.Investment.AHP.Contract.Delivery;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public record SummaryOfDeliveryTrancheModel(
    SummaryOfDeliveryTrancheType TrancheType,
    string? Value,
    string DeliveryPhaseId,
    string DeliveryPhaseName,
    string ApplicationName);
