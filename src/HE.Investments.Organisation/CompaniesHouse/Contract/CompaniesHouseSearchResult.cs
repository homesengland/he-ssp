namespace HE.Investments.Organisation.CompaniesHouse.Contract;

public class CompaniesHouseSearchResult
{
    public IList<CompanyDetailsItem> Items { get; set; }

    public int Hits { get; set; }
}
