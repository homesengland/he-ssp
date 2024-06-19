using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

internal sealed class IsThereProjectWithNameResponse
{
    [JsonPropertyName("invln_frontdoorprojectexists")]
    public bool Exists { get; set; }
}
