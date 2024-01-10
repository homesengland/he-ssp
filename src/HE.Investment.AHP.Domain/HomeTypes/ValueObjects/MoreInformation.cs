using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MoreInformation : LongText
{
    public MoreInformation(string value, string fieldDisplayName = "more information")
        : base(value, nameof(MoreInformation), fieldDisplayName)
    {
    }
}
