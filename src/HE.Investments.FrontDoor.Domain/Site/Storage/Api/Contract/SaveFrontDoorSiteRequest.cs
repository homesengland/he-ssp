using System.Text.Json.Serialization;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;

internal sealed class SaveFrontDoorSiteRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; set; }

    [JsonPropertyName("invln_frontdoorsiteid")]
    public string SiteId { get; set; }

    [JsonPropertyName("invln_entityfieldsparameters")]
    public string Site { get; set; }
}
