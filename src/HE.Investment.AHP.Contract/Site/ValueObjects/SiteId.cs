using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site.ValueObjects;

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

    public override string ToString()
    {
        return Value;
    }
}
