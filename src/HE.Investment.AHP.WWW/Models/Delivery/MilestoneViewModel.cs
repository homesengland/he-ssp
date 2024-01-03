namespace HE.Investment.AHP.WWW.Models.Delivery;
public record MilestoneViewModel(string? ApplicationId, string? ApplicationName, string? DeliveryPhaseName, MilestoneDatesModel? MilestoneDates)
    : DeliveryViewModelBase(ApplicationId, ApplicationName, DeliveryPhaseName);
