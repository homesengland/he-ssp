namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract.Responses;

public sealed record GetSiteResponse(
    DateTimeOffset CreatedOn,
    string? LocalAuthority,
    string? LocalAuthorityName,
    string? SiteId,
    string SiteName,
    int? NumberOfHomesEnabledBuilt,
    int? PlanningStatus,
    string? LocalAuthorityCode,
    string ProjectRecordId);
