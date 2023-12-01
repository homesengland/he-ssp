using HE.Investments.Loans.Contract.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MoreInformation : LongText
{
    public MoreInformation(string value, string fieldDisplayName = "More information")
        : base(value, nameof(MoreInformation), fieldDisplayName)
    {
    }
}
