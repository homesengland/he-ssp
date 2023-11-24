using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investment.AHP.WWW.Models.Common;

public static class CommonFormOptions
{
    public static IEnumerable<SelectListItem> YesNo => new List<SelectListItem>
    {
        new SelectListItem { Text = CommonResponse.Yes, Value = CommonResponse.Yes },
        new SelectListItem { Text = CommonResponse.No, Value = CommonResponse.No },
    };
}
