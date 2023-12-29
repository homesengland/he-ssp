namespace HE.Investments.Common.WWW.Models;

public record ActionModel(string Label, string Action, string Controller, object? Values = null, bool HasAccess = false);
