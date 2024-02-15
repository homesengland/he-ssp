using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;

public class ModernMethodsOfConstructionFutureAdoption : ValueObject, IQuestion
{
    public ModernMethodsOfConstructionFutureAdoption(ModernMethodsOfConstructionPlans plans, ModernMethodsOfConstructionExpectedImpact expectedImpact)
    {
        Plans = plans;
        ExpectedImpact = expectedImpact;
    }

    public ModernMethodsOfConstructionPlans Plans { get; }

    public ModernMethodsOfConstructionExpectedImpact ExpectedImpact { get; }

    public static ModernMethodsOfConstructionFutureAdoption? Create(ModernMethodsOfConstructionPlans plans, ModernMethodsOfConstructionExpectedImpact expectedImpact)
    {
        if (plans.Value.IsNotProvided() && expectedImpact.Value.IsNotProvided())
        {
            return null;
        }

        return new ModernMethodsOfConstructionFutureAdoption(plans, expectedImpact);
    }

    public bool IsAnswered()
    {
        return Plans.IsProvided() && ExpectedImpact.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Plans;
        yield return ExpectedImpact;
    }
}
