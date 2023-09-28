using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Organization.ValueObjects;
public class OrganizationBasicDetails : ValueObject
{
    public OrganizationBasicDetails(string name, string street, string city, string postalCode, string companiesHouseNumber)
    {
        Name = name;
        Street = street;
        City = city;
        PostalCode = postalCode;
        CompaniesHouseNumber = companiesHouseNumber;
    }

    public string Name { get; }

    public string Street { get; }

    public string City { get; }

    public string PostalCode { get; }

    public string CompaniesHouseNumber { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
        yield return Street;
        yield return City;
        yield return PostalCode;
        yield return CompaniesHouseNumber;
    }
}
