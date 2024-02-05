namespace HE.Investment.AHP.WWW.Models.Delivery;

public record SummaryOfDeliveryRow(string ValueName, decimal? Value, decimal? Percentage, string Id, string ApplicationId, bool IsAmendable);
