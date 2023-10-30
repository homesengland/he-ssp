using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Application;

public static class Tenures
{
    public static List<SelectListItem> AvailableTenures { get; } = new()
    {
        new() { Value = "AffordableRent", Text = "Affordable Rent", },
        new() { Value = "SocialRent", Text = "Social Rent", },
        new() { Value = "SharedOwnership", Text = "Shared Ownership", },
        new() { Value = "RentToBuy", Text = "Rent to Buy", },
        new() { Value = "HomeOwnershipLongTermDisabilities", Text = "Home Ownership for People with Long-term Disabilities (HOLD)", },
        new() { Value = "OlderPersonsSharedOwnership", Text = "Older Persons Shared Ownership (OPSO)", },
    };
}
