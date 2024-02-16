using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionInformation : ValueObject, IQuestion
{
    public ModernMethodsOfConstructionInformation(ModernMethodsOfConstructionBarriers? barriers, ModernMethodsOfConstructionImpact? impact)
    {
        Barriers = barriers;
        Impact = impact;
    }

    public ModernMethodsOfConstructionBarriers? Barriers { get; }

    public ModernMethodsOfConstructionImpact? Impact { get; }

    public static ModernMethodsOfConstructionInformation? Create(ModernMethodsOfConstructionBarriers? barriers, ModernMethodsOfConstructionImpact? impact)
    {
        if (barriers.IsNotProvided() && impact.IsNotProvided())
        {
            return null;
        }

        return new ModernMethodsOfConstructionInformation(barriers, impact);
    }

    public bool IsAnswered()
    {
        return Barriers.IsProvided() && Impact.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Barriers;
        yield return Impact;
    }
}
