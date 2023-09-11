using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Security.ValueObjects;

public class Debenture : ValueObject
{
    public Debenture(string holder, bool exists)
    {
        Holder = holder;
        Exists = exists;
    }

    public string Holder { get; }

    public bool Exists { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Holder;
        yield return Exists;
    }
}
