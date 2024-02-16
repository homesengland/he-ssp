using HE.Investment.AHP.Contract.Delivery.MilestonePayments;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseTranchesDto(bool ShouldBeAmended, SummaryOfDelivery? SummaryOfDelivery, SummaryOfDeliveryAmend? SummaryOfDeliveryAmend);
