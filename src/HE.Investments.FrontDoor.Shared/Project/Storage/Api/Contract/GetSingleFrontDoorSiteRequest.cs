using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetSingleFrontDoorSiteRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; init; }

    [JsonPropertyName("invln_frontdoorprojectsiteid")]
    public string SiteId { get; init; }
}
