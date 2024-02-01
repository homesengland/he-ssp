using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Site;

public static class SiteFormOptions
{
    public static IEnumerable<SelectListItem> NationalDesignGuidePrioritiesExceptNone => SelectListHelper.FromEnum<NationalDesignGuidePriority>()
        .Where(x => x.Value != NationalDesignGuidePriority.NoneOfTheAbove.ToString());

    public static SelectListItem NationalDesignGuidePrioritiesOnlyNone => SelectListHelper.FromEnum(NationalDesignGuidePriority.NoneOfTheAbove);
}
