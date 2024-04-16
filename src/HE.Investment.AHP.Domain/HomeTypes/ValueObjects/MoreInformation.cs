using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MoreInformation : LongText
{
    public MoreInformation(string value, string fieldDisplayName)
        : base(value, nameof(MoreInformation), fieldDisplayName)
    {
    }

    public static MoreInformation? FromCrm(string? value) => value.IsProvided() ? new MoreInformation(value!, "more information") : null;
}
