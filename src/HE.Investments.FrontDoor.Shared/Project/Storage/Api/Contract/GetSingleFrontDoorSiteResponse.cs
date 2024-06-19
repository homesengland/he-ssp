using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetSingleFrontDoorSiteResponse
{
    [JsonPropertyName("invln_frontdoorprojectsite")]
    public string Site { get; set; }
}
