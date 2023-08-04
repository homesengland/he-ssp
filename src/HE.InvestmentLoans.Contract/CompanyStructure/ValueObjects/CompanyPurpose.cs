using HE.InvestmentLoans.Contract.Domain;

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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsSpv;
    }
}
