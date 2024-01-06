using Dawn;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.Site.ValueObjects;

public class SiteId : StringIdValueObject
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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
