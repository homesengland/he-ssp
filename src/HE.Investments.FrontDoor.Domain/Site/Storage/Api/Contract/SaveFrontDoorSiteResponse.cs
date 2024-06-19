using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;

internal sealed class SaveFrontDoorSiteResponse
{
    [JsonPropertyName("invln_frontdoorsiteid")]
    public string SiteId { get; set; }
}
