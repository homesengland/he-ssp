namespace HE.Investments.Common.WWW.Routing;

public record Breadcrumb(string Text, string? Action = null, string? Controller = null, object? Params = null);
