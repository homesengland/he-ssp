namespace HE.Investments.FrontDoor.WWW.Models;

public record LocalAuthorityViewModel(string LocalAuthorityCode, string LocalAuthorityName, string ProjectId, string? SiteId = null, bool? IsConfirmed = null);
