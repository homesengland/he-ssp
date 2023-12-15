using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Common;

public static class CommonFormOptions
{
    public static IEnumerable<SelectListItem> YesNo => SelectListHelper.FromEnum<YesNoType>();

    public static IEnumerable<SelectListItem> IsSectionCompleted => SelectListHelper.FromEnum<IsSectionCompleted>();

    public static IEnumerable<ExtendedSelectListItem> IsSectionCompletedExtended => ExtendedSelectListHelper.FromEnum<IsSectionCompleted>();
}
