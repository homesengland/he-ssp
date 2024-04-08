using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionPlans : YourLongText
{
    public ModernMethodsOfConstructionPlans(string? value)
        : base(value, "FutureAdoptionPlans", "plans for adopting Modern Methods of Construction in the future")
    {
    }

    public static ModernMethodsOfConstructionPlans? Create(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : new ModernMethodsOfConstructionPlans(value);
    }
}
