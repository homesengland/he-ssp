using HE.Investments.Common.Contract;

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

    public static AhpApplicationId From(string value) => new(value);

    public static AhpApplicationId From(Guid value) => new(value.ToString());

    public override string ToString()
    {
        return Value;
    }
}
