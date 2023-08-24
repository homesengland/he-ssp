using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.InvestmentLoans.Contract.Organization;
public class OrganizationViewModel
{
    public OrganizationViewModel()
    {
        Organizations = new List<OrganizationBasicDetails>();
    }

    public OrganizationViewModel(IEnumerable<OrganizationBasicDetails> organizations)
    {
        Organizations = organizations.ToList();
    }

    public string Name { get; set; }

    public OrganizationBasicDetails SelectedOrganization { get; set; }

    public IEnumerable<OrganizationBasicDetails> Organizations { get; set; }

    public int TotalOrganizations { get; set; }

    public int Page { get; set; }
}
