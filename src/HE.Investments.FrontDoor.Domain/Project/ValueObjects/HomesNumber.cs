using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class HomesNumber : TheRequiredIntValueObject, IQuestion
{
    private const string DisplayName = "number of homes your project will enable";

    private const int MinValue = 0;

    private const int MaxValue = 9999;

    public HomesNumber(string? value, string? displayName = null)
        : base(value, nameof(HomesNumber), displayName ?? DisplayName, MinValue, MaxValue)
    {
    }

    public HomesNumber(int value, string? displayName = null)
        : base(value, nameof(HomesNumber), displayName ?? DisplayName, MinValue, MaxValue)
    {
    }

    public bool IsAnswered()
    {
        return Value.IsProvided();
    }
}
