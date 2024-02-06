using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Site;

public static class SiteFormOptions
{
    public static IEnumerable<ExtendedSelectListItem> NationalDesignGuidePrioritiesExceptNone => SelectListHelper.FromEnumToExtendedList<NationalDesignGuidePriority>()
        .Where(x => x.Value != NationalDesignGuidePriority.NoneOfTheAbove.ToString());

    public static ExtendedSelectListItem NationalDesignGuidePrioritiesOnlyNone => SelectListHelper.FromEnumToExtendedListItem(NationalDesignGuidePriority.NoneOfTheAbove);
}
