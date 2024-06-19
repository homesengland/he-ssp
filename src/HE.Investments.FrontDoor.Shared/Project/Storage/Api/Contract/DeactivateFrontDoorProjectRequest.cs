using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class DeactivateFrontDoorProjectRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; init; }
}
