using System.Text.Json.Serialization;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api.Contract;

internal sealed class IsThereProjectWithNameRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_organisationid")]
    public string OrganisationId { get; init; }

    [JsonPropertyName("invln_frontdoorprojectname")]
    public string ProjectName { get; init; }
}
