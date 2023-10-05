using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Tests.TestObjectBuilders;

public class CompanyDetailsItemTestBuilder
{
    private readonly CompanyDetailsItem _item;

    private CompanyDetailsItemTestBuilder(string companyHouseNumber)
    {
        _item = new CompanyDetailsItem
        {
            CompanyNumber = companyHouseNumber,
            CompanyName = "CompanyHouse Company" + companyHouseNumber,
            OfficeAddress = new OfficeAddress
            {
                AddressLine1 = "AddressLine1",
                AddressLine2 = "AddressLine2",
                Country = "Country",
                Locality = "Locality",
                PostalCode = "PostalCode",
                Region = "Region",
            }
        };
    }

    public static CompanyDetailsItemTestBuilder New(string companyHouseNumber = "1234567") => new(companyHouseNumber);

    public CompanyDetailsItem Build() => _item;
}
