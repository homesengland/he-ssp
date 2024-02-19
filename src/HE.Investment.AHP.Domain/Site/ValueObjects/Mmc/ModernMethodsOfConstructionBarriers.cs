using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionBarriers : LongText
{
    public ModernMethodsOfConstructionBarriers(string? value)
        : base(value, "ModernMethodsOfConstructionBarriers", "barriers you have experienced or foresee yourself experiencing in introducing greater levels of MMC into your development programme")
    {
    }

    public static ModernMethodsOfConstructionBarriers? Create(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : new ModernMethodsOfConstructionBarriers(value);
    }
}
