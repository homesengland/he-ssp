namespace HE.Investments.FrontDoor.Domain.Project.Api.Contract.Responses;

internal sealed record GetSiteResponse(
    DateTimeOffset CreatedOn,
    string? LocalAuthority,
    string? LocalAuthorityName,
    string? SiteId,
    string SiteName,
    int? NumberOfHomesEnabledBuilt,
    int? PlanningStatus,
    string? LocalAuthorityCode,
    string ProjectRecordId);
