using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Common.Extensions;

public static class PoundsValueObjectExtensions
{
    public static decimal GetValueOrZero(this PoundsValueObject? valueObject)
    {
        return valueObject?.Value ?? 0;
    }
}
