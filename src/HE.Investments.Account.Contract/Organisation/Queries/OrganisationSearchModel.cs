namespace HE.Investments.Account.Contract.Organisation.Queries;

public class OrganisationSearchModel
{
    public OrganisationSearchModel()
    {
        Organisations = [];
    }

    public OrganisationSearchModel(IEnumerable<OrganisationBasicDetails> organisations)
    {
        Organisations = organisations.ToList();
    }

    public string Name { get; set; }

    public string SelectedOrganisation { get; set; }

    public IList<OrganisationBasicDetails> Organisations { get; set; }

    public int TotalOrganisations { get; set; }

    public int Page { get; set; }

    public int ItemsPerPage { get; set; }
}
