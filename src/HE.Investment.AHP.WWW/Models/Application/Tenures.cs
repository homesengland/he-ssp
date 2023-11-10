using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Application;

public static class Tenures
{
    public static IEnumerable<SelectListItem> AvailableTenures => SelectListHelper.FromEnum<Tenure>();
}
