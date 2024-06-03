namespace HE.Investment.AHP.Contract.Site;

public record SiteBasicModel(string Id, string Name, string ProjectId, string? LocalAuthorityName, SiteStatus Status);
