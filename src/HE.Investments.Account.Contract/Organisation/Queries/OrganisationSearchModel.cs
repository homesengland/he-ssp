using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public class OrganisationSearchModel
{
    public OrganisationSearchModel()
    {
        Organizations = new List<OrganizationBasicDetails>();
    }

    public OrganisationSearchModel(IEnumerable<OrganizationBasicDetails> organizations)
    {
        Organizations = organizations.ToList();
    }

    public string Name { get; set; }

    public string SelectedOrganization { get; set; }

    public IEnumerable<OrganizationBasicDetails> Organizations { get; set; }

    public int TotalOrganizations { get; set; }

    public int Page { get; set; }

    public int ItemsPerPage { get; set; }
}
