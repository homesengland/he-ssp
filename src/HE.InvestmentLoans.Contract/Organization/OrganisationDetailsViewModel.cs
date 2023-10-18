using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.InvestmentLoans.Contract.Organization;
public class OrganisationDetailsViewModel
{
    public OrganisationDetailsViewModel()
    {
        AddressLines = new List<string>();
    }

    public OrganisationDetailsViewModel(string name, string phoneNumber, IEnumerable<string> addresLines, string houseNumber, OrganisationChangeRequestState changeRequestState)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        ChangeRequestState = changeRequestState;
        AddressLines = addresLines;
        HouseNumber = houseNumber;
        ChangeRequestState = changeRequestState;
    }

    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public IEnumerable<string> AddressLines { get; set; }

    public string HouseNumber { get; set; }

    public OrganisationChangeRequestState ChangeRequestState { get; set; }
}
