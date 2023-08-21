using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;
public class OrganizationViewModel
{
    public OrganizationViewModel()
    {
        Organizations = new List<OrganizationBasicDetails>();
        MockData();
        MapOrganizationsToSelectList();
    }

    public string Name { get; set; }

    public OrganizationBasicDetails SelectedOrganization { get; set; }

    public List<OrganizationBasicDetails> Organizations { get; set; }

    public List<SelectListItem> OrganizationSelectListItems { get; set; }

    private void MockData()
    {
        Organizations.Add(new OrganizationBasicDetails("ABC limited", "Tree walk place", "London", "E1 6EF", "1111111"));
        Organizations.Add(new OrganizationBasicDetails("ABC developments", "Tooley St", "Southwark", "SE1 7BR", "2222222"));
    }

    private void MapOrganizationsToSelectList()
    {
        OrganizationSelectListItems = Organizations.Select(org => new SelectListItem
        {
            Text = $"{org.Name}<br>{org.Street}<br>{org.City}<br>{org.Code}<br>Companies house number: {org.CompaniesHouseNumber}",
            Value = org.Name,
        }).ToList();
    }
}
