using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetMultipleFrontDoorSitesRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; init; }

    [JsonPropertyName("invln_pagingrequest")]
    public string PagingRequest { get; init; }
}
