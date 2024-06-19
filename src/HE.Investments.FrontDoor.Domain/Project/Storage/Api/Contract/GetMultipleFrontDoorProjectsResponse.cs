using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

internal sealed class GetMultipleFrontDoorProjectsResponse
{
    [JsonPropertyName("invln_frontdoorprojects")]
    public string Projects { get; set; }
}
