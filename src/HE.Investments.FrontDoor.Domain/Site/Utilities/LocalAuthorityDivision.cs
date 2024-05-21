using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Utilities.Enums;

namespace HE.Investments.FrontDoor.Domain.Site.Utilities;

public static class LocalAuthorityDivision
{
    public static bool IsLocalAuthorityNotAllowedForLoanApplication(string? localAuthorityCode)
    {
        return Enum.GetValues<LoanApplicationNotAllowedLocalAuthority>()
            .Select(x => x.GetDescription())
            .Any(x => localAuthorityCode?.Contains(x) == true);
    }

    public static bool IsLocalAuthorityNotAllowedForAhpProject(string? localAuthorityCode)
    {
        return Enum.GetValues<AhpProjectNotAllowedLocalAuthority>()
            .Select(x => x.GetDescription())
            .Any(x => localAuthorityCode?.Contains(x) == true);
    }
}
