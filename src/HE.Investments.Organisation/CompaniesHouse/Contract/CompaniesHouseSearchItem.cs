using System.Text.Json.Serialization;

namespace HE.Investments.Organisation.CompaniesHouse.Contract;

public class CompaniesHouseSearchItem
{
    [JsonPropertyName("company_number")]
    public string CompanyNumber { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}
