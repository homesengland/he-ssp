using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

internal sealed class SaveFrontDoorProjectResponse
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; set; }
}
