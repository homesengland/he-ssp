using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Models;

public static class FormOptions
{
    public static List<SelectListItem> YesNo { get; } =
    [
        new SelectListItem { Value = "Yes", Text = "Yes", },
        new SelectListItem { Value = "No", Text = "No", }
    ];
}
