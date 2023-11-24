using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeId : ValueObject
{
    public HomeTypeId(string id)
    {
        Value = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
    }

    private HomeTypeId()
    {
        Value = string.Empty;
    }

    public string Value { get; }

    public bool IsNew => string.IsNullOrEmpty(Value);

    public static HomeTypeId New() => new();

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
