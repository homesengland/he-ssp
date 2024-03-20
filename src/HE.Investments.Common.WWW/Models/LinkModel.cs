namespace HE.Investments.Common.WWW.Models;

public record LinkModel(ActionLinkModel ActionLink, DirectLinkModel DirectLink);

public record ActionLinkModel(string Action, string Controller, object Values);

public record DirectLinkModel(string DirectLink, bool OpenInNewTab = false);
