using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class EnvironmentalImpact : LongText
{
    public EnvironmentalImpact(string? value)
        : base(value, "EnvironmentalImpact", "environmental impact")
    {
    }
}
