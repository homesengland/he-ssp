using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionPlans : LongText
{
    public ModernMethodsOfConstructionPlans(string? value)
        : base(value, "ModernMethodsOfConstructionPlans", "plans for adopting Modern Methods of Construction in the future")
    {
    }
}
