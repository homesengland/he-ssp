using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.Application;

public record AhpApplicationId : StringIdValueObject
{
    public AhpApplicationId(string id)
        : base(id)
    {
    }

    private AhpApplicationId()
    {
    }

    public static AhpApplicationId New() => new();

    public static AhpApplicationId From(string value) => new(FromStringToShortGuidAsString(value));

    public static AhpApplicationId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
