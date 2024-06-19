using System.Text.Json.Serialization;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract;

internal sealed class RemoveFrontDoorSiteRequest : HeTablesRequestBase
{
    [JsonPropertyName("invln_frontdoorsiteid")]
    public string SiteId { get; set; }
}
