namespace HE.Investment.AHP.WWW.Models.Delivery;

public record AddHomesModel(
        string ApplicationId,
        string ApplicationName,
        string DeliveryPhaseName,
        IDictionary<string, string> HomeTypes,
        IDictionary<string, string?> HomesToDeliver)
    : DeliveryViewModelBase(ApplicationId, ApplicationName, DeliveryPhaseName);
