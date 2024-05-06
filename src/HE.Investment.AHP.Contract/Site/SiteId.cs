using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.Site;

public record SiteId : StringIdValueObject
{
    public SiteId(string id)
        : base(id)
    {
    }

    private SiteId()
    {
    }

    public static SiteId New() => new();

    public static SiteId From(string value) => new(FromStringToShortGuidAsString(value));

    public static SiteId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public static SiteId? Create(string? id) => id.IsProvided() ? From(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
