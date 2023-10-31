using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Security.ValueObjects;

public class DirectorLoans : ValueObject
{
    public DirectorLoans(bool exists)
    {
        Exists = exists;
    }

    public bool Exists { get; }

    public static DirectorLoans FromString(string exists) =>
        new(exists.MapToNonNullableBool());

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Exists;
    }
}
