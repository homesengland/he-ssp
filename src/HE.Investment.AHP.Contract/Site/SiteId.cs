using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.Site;

public record SiteId : StringIdValueObject
{
    public SiteId(string value)
        : base(value)
    {
    }

    private SiteId()
    {
    }

    public static SiteId New() => new();

    public static SiteId From(string value) => new(FromStringToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public static SiteId? Create(string? id) => id.IsProvided() ? From(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
