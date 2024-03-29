using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Utilities.Enums;

namespace HE.Investments.FrontDoor.Domain.Site.Utilities;

public static class LocalAuthorityDivision
{
    public static bool IsLocalAuthorityNotAllowedForLoanApplication(string? localAuthorityCode)
    {
        return Enum.GetValues<NotAllowedLocalAuthority>()
            .Select(x => x.GetDescription())
            .Any(x => localAuthorityCode?.Contains(x) == true);
    }
}
