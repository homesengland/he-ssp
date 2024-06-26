using System.Globalization;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
internal static class GrantFundingStatusMapper
{
    public static PublicSectorGrantFundingStatus? FromString(string value)
    {
        if (value.IsNotProvided())
        {
            return null;
        }

        return value!.ToLower(CultureInfo.InvariantCulture) switch
        {
            CommonResponse.Lowercase.Yes => PublicSectorGrantFundingStatus.Received,
            CommonResponse.Lowercase.No => PublicSectorGrantFundingStatus.NotReceived,
            CommonResponse.Lowercase.DoNotKnow => PublicSectorGrantFundingStatus.Unknown,
            _ => null,
        };
    }

    public static string ToString(PublicSectorGrantFundingStatus status)
    {
        return status switch
        {
            PublicSectorGrantFundingStatus.Received => CommonResponse.Yes,
            PublicSectorGrantFundingStatus.NotReceived => CommonResponse.No,
            PublicSectorGrantFundingStatus.Unknown => CommonResponse.DoNotKnow,
            _ => throw new ArgumentException($"Cannot convert {status} to PublicSectorGrantFundingStatus."),
        };
    }
}
