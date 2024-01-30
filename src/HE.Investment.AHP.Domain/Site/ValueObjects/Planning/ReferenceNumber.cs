using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class ReferenceNumber : ShortText
{
    public ReferenceNumber(string? value)
        : base(value, "ReferenceNumber", "planning reference number")
    {
    }

    public static ReferenceNumber? Create(string? value) => !string.IsNullOrWhiteSpace(value) ? new ReferenceNumber(value) : null;
}
