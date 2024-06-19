using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public sealed class GetSingleFrontDoorProjectRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_organisationid")]
    public string OrganisationId { get; init; }

    [JsonPropertyName("invln_userid")]
    public string UserId { get; init; }

    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; init; }
}
