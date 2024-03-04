using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public record SiteId : StringIdValueObject
{
    // TODO: #87404 remove - added for backwards compatibility
    public SiteId(string id)
        : base(string.IsNullOrEmpty(id) ? "9b1f7a4e-98d0-ee11-9079-002248004f63" : id)
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
