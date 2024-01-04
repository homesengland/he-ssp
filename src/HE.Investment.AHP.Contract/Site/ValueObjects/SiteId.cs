using Dawn;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.Site.ValueObjects;

public class SiteId : ValueObject
{
    private static readonly string NotSaved = "new";

    public SiteId(string id)
    {
        if (id == NotSaved)
        {
            Value = string.Empty;
            return;
        }

        Value = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
    }

    public string Value { get; }

    public static SiteId New() => new(NotSaved);

    public bool IsNew() => Value.IsNotProvided();

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
