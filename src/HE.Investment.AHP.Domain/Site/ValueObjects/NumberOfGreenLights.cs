using System.Globalization;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class NumberOfGreenLights : TheRequiredIntValueObject
{
    private const string DisplayName = "value you enter for the Building for Life green traffic lights";

    private const int MinValue = 0;

    private const int MaxValue = 12;

    public NumberOfGreenLights(string value)
        : base(value, nameof(NumberOfGreenLights), DisplayName, MinValue, MaxValue, MessageOptions.HideExample)
    {
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
