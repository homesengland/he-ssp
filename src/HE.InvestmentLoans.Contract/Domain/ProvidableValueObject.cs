using System.Diagnostics.CodeAnalysis;

namespace HE.InvestmentLoans.Contract.Domain;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Allowed for core classes")]
public abstract class ProvidableValueObject<T> : ValueObject
    where T : ValueObject
{
    public static Providable<T> NotProvided() => Providable<T>.NotProvided();
}
