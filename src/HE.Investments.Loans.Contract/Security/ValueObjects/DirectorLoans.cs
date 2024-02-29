using HE.Investments.Common.Domain;
using HE.Investments.Common.WWW.Extensions;

namespace HE.Investments.Loans.Contract.Security.ValueObjects;

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
