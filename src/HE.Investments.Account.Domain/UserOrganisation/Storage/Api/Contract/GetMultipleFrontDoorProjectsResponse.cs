using System.Text.Json.Serialization;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage.Api.Contract;

internal sealed class GetMultipleFrontDoorProjectsResponse
{
    [JsonPropertyName("invln_frontdoorprojects")]
    public string Projects { get; set; }
}
