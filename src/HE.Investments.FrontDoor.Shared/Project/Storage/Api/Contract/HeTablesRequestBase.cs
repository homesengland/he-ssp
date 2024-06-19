using System.Text.Json.Serialization;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract;

public class HeTablesRequestBase
{
    [JsonPropertyName("invln_usehetables")]
    public string UseHeTables => "true";
}
