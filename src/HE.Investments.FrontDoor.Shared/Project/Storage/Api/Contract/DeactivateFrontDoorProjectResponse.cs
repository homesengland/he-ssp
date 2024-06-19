using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class DeactivateFrontDoorProjectResponse
{
    [JsonPropertyName("invln_projectdeactivated")]
    public string ProjectDeactivated { get; set; }
}
