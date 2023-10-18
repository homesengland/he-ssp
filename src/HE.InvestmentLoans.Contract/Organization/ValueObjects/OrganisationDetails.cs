using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Organization.ValueObjects;
public class OrganisationDetails : ValueObject
{
    public OrganisationDetails(string name, string street, string district, string city, string postalCode, string phoneNumber, string companiesHouseNumber, string changeRequestState)
    {
        Name = name;
        Street = street;
        District = district;
        City = city;
        PostalCode = postalCode;
        CompaniesHouseNumber = companiesHouseNumber;
        ChangeRequestState = changeRequestState;
        PhoneNumber = phoneNumber;
    }

    public string Name { get; }

    public string Street { get; }

    public string District { get; }

    public string City { get; }

    public string PostalCode { get; }

    public string CompaniesHouseNumber { get; }

    public string ChangeRequestState { get; }

    public string PhoneNumber { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
        yield return Street;
        yield return District;
        yield return City;
        yield return PostalCode;
        yield return CompaniesHouseNumber;
        yield return ChangeRequestState;
        yield return PhoneNumber;
    }
}
