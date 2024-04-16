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

    public static SiteId? Create(string? id) => id.IsProvided() ? new SiteId(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
