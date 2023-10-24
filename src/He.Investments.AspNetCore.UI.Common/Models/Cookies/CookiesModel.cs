namespace He.Investments.AspNetCore.UI.Common.Models.Cookies;

public record CookiesModel(IList<CookieModel> EssentialCookies, IList<CookieModel> AdditionalCookies);

public record CookieModel(string Name, string Purpose, string Expires);
