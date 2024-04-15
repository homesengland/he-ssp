using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Common.Extensions;

public static class TheRequiredIntValueObjectExtensions
{
    public static decimal GetValueOrZero(this TheRequiredIntValueObject? valueObject)
    {
        return valueObject?.Value ?? 0;
    }
}
