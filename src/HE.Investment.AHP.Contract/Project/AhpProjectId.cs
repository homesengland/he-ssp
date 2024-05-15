using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Project;

public record AhpProjectId : StringIdValueObject
{
    public AhpProjectId(string value)
        : base(value)
    {
    }

    private AhpProjectId()
    {
    }

    public static AhpProjectId New() => new();

    public static AhpProjectId From(string value) => new(value);

    public static AhpProjectId From(Guid value) => new(value.ToString());

    public override string ToString()
    {
        return Value;
    }
}
