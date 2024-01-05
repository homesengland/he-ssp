using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.Delivery;
public record MilestoneViewModel(string? ApplicationId, string? ApplicationName, string? DeliveryPhaseName, DateDetails MilestoneStartAt, DateDetails ClaimMilestonePaymentAt)
    : DeliveryViewModelBase(ApplicationId, ApplicationName, DeliveryPhaseName);
