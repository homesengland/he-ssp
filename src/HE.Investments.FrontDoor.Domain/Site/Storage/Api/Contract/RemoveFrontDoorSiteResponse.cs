using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;

internal sealed class RemoveFrontDoorSiteResponse
{
    [JsonPropertyName("invln_sitedeactivated")]
    public string Response { get; set; }
}
