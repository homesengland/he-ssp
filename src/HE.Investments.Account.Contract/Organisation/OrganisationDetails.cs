namespace HE.Investments.Account.Contract.Organisation;

public class OrganisationDetails
{
    public OrganisationDetails()
    {
        AddressLines = new List<string>();
    }

    public OrganisationDetails(
        string name,
        string phoneNumber,
        IEnumerable<string> addressLines,
        string houseNumber,
        OrganisationChangeRequestState changeRequestState,
        InvestmentPartnerStatus? investmentPartnerStatus,
        bool hasAnyAhpApplication)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        AddressLines = addressLines;
        HouseNumber = houseNumber;
        ChangeRequestState = changeRequestState;
        InvestmentPartnerStatus = investmentPartnerStatus;
        HasAnyAhpApplication = hasAnyAhpApplication;
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

    public InvestmentPartnerStatus? InvestmentPartnerStatus { get; set; }

    public bool HasAnyAhpApplication { get; set; }
}
