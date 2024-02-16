using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionExpectedImpact : LongText
{
    public ModernMethodsOfConstructionExpectedImpact(string? value)
        : base(value, "ModernMethodsOfConstructionExpectedImpact", "impact you think this would have on your development programme")
    {
    }

    public static ModernMethodsOfConstructionExpectedImpact? Create(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : new ModernMethodsOfConstructionExpectedImpact(value);
    }
}
