using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class CompanyPurpose : ValueObject
{
    protected CompanyPurpose(bool isSpv)
    {
        IsSpv = isSpv;
    }

    /// <summary>
    /// An SPV is a limited company set up only for the purpose of holding property and for carrying out buy-to-let activities.
    /// </summary>
    public bool IsSpv { get; }

    public static CompanyPurpose New(bool isSpv) => new(isSpv);

    public static CompanyPurpose FromString(string companyPurposeAsString)
    {
        return new CompanyPurpose(companyPurposeAsString == CommonResponse.Yes);
    }

    public override string ToString()
    {
        return IsSpv.ToString().ToLowerInvariant();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsSpv;
    }
}
