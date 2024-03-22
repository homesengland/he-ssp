using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Utilities.Enums;

namespace HE.Investments.FrontDoor.Domain.Site.Utilities;

public static class LocalAuthorityDivision
{
    public static bool IsLocalAuthorityNotAllowedForLoanApplication(string? localAuthorityCode)
    {
#pragma warning disable S6605
        return Enum.GetValues<NotAllowedLocalAuthority>().Any(x => x.GetDescription() == localAuthorityCode);
#pragma warning restore S6605
    }
}
