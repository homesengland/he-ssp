namespace HE.Investment.AHP.WWW.Models.Delivery;

public record DeliveryRequestAdditionalPaymentViewModel(string? ApplicationId, string? ApplicationName, string? DeliveryPhaseName, bool? IsAdditionalPaymentRequested)
    : DeliveryViewModelBase(ApplicationId, ApplicationName, DeliveryPhaseName);
