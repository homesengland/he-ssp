using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetMultipleFrontDoorSitesResponse
{
    [JsonPropertyName("invln_frontdoorprojectsites")]
    public string Sites { get; set; }
}
