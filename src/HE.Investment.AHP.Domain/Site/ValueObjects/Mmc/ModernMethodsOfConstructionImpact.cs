using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionImpact : LongText
{
    public ModernMethodsOfConstructionImpact(string? value)
        : base(value, "ModernMethodsOfConstructionImpact", "impact the use of MMC has had on your development to date")
    {
    }

    public static ModernMethodsOfConstructionImpact? Create(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : new ModernMethodsOfConstructionImpact(value);
    }
}
