using HE.InvestmentLoans.Contract.Organization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.InvestmentLoans.WWW.Helpers.Organisation;

public static class OrganizationsMapper
{
    public static List<SelectListItem> MapToSelectListItems(OrganizationViewModel viewModel)
    {
        return viewModel.Organizations.Select(org => new SelectListItem
        {
            Text = $"{ToHtml(org.Name)}{ToHtml(org.Street)}{ToHtml(org.City)}{ToHtml(org.Code)}Companies house number: {org.CompaniesHouseNumber}",
            Value = org.Name,
        }).ToList();
    }

    private static string ToHtml(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return string.Empty;
        }

        return $"{data}<br>";
    }
}
