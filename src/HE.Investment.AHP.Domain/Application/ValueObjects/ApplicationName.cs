using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationName : YourShortText
{
    public ApplicationName(string? value)
        : base(value, "Name", "application name")
    {
    }
}
