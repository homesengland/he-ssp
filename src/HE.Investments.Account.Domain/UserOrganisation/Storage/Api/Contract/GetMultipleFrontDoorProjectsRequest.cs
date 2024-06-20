using System.Text.Json.Serialization;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Api.Contract;

internal sealed class GetMultipleFrontDoorProjectsRequest
{
    [JsonPropertyName("invln_organisationid")]
    public string OrganisationId { get; init; }

    [JsonPropertyName("invln_userid")]
    public string UserId { get; init; }

    [JsonPropertyName("invln_usehetables")]
    public string UseHeTables => "true";
}
