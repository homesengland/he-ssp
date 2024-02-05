using HE.Investment.AHP.Contract.Delivery.MilestonePayments;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseTranchesDto(bool IsAmendable, SummaryOfDelivery? SummaryOfDelivery, SummaryOfDeliveryAmend? SummaryOfDeliveryAmend);
