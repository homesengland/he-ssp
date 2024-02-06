using HE.Investment.AHP.Contract.Delivery;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public record SummaryOfDeliveryTrancheModel(
    SummaryOfDeliveryTrancheType TrancheType,
    decimal? Value,
    string DeliveryPhaseId,
    string DeliveryPhaseName,
    string ApplicationName);
