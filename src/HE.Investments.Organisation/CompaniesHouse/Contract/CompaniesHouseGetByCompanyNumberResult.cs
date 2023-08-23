using System.Text.Json.Serialization;

namespace HE.Investments.Organisation.CompaniesHouse.Contract;

public class CompaniesHouseGetByCompanyNumberResult
{
    [JsonPropertyName("company_number")]
    public string CompanyNumber { get; set; }

    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; }

    [JsonPropertyName("registered_office_address")]
    public OfficeAddress OfficeAddress { get; set; }
}
