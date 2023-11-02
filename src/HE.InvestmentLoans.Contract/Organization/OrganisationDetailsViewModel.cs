using HE.InvestmentLoans.Common.Utils.Enums;

namespace HE.InvestmentLoans.Contract.Organization;

public class OrganisationDetailsViewModel
{
    public OrganisationDetailsViewModel()
    {
        AddressLines = new List<string>();
    }

    public OrganisationDetailsViewModel(
        string name,
        string phoneNumber,
        IEnumerable<string> addressLines,
        string houseNumber,
        OrganisationChangeRequestState changeRequestState)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        AddressLines = addressLines;
        HouseNumber = houseNumber;
        ChangeRequestState = changeRequestState;
    }

    public OrganisationDetailsViewModel(
        string name,
        string phoneNumber,
        string addressLine1,
        string addressLine2,
        string townOrCity,
        string county,
        string postcode)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        TownOrCity = townOrCity;
        County = county;
        Postcode = postcode;
    }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public IEnumerable<string> AddressLines { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? TownOrCity { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public string HouseNumber { get; set; }

    public OrganisationChangeRequestState ChangeRequestState { get; set; }
}
