namespace HE.Investments.FrontDoor.Domain.Site.Api.Contract.Requests;

internal sealed record SaveSiteRequest
{
    public string ProjectRecordId { get; init; }

    public string? SiteId { get; init; }

    public string SiteName { get; init; }

    public int? NumberOfHomesEnabledBuilt { get; init; }

    public int? PlanningStatus { get; init; }

    public string? LocalAuthorityCode { get; init; }
}
