using System.Text.Json.Serialization;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

internal sealed class SaveFrontDoorProjectRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorprojectid")]
    public string ProjectId { get; init; }

    [JsonPropertyName("invln_organisationid")]
    public string OrganisationId { get; init; }

    [JsonPropertyName("inlvn_userid")]
    public string UserId { get; init; }

    [JsonPropertyName("invln_entityfieldsparameters")]
    public string Project { get; init; }
}
