using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetSingleFrontDoorProjectResponse
{
    [JsonPropertyName("invln_retrievedfrontdoorprojectfields")]
    public string Projects { get; set; }
}
