namespace HE.Investments.Organisation.CompaniesHouse.Contract;

public class CompaniesHouseSearchResult
{
    public IList<CompanyDetailsItem> Items { get; set; }

    public int Hits { get; set; }

    public static CompaniesHouseSearchResult New(IList<CompanyDetailsItem> items, int hits) =>
        new()
        {
            Hits = hits,
            Items = items,
        };
}
