namespace HE.Investments.Common.WWW.Models.Cookies;

public record CookiesModel(IList<CookieModel> EssentialCookies, IList<CookieModel> AdditionalCookies);

public record CookieModel(string Name, string Purpose, string Expires);
