namespace HE.Investments.Organisation.CompaniesHouse;

public class CompaniesHouseConfig : ICompaniesHouseConfig
{
    public Uri CompaniesHouseBaseUrl { get; set; }

    public string AuthorizationKey { get; set; }
}
