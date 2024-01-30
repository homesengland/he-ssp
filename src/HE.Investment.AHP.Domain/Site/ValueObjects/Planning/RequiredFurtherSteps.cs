using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class RequiredFurtherSteps : LongText
{
    public RequiredFurtherSteps(string? value)
        : base(value, "RequiredFurtherSteps", "further steps required")
    {
    }

    public static RequiredFurtherSteps? Create(string? value) => !string.IsNullOrWhiteSpace(value) ? new RequiredFurtherSteps(value) : null;
}
