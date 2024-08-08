using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

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

    public static AhpProjectId From(string value) => new(FromStringToShortGuidAsString(value));

    public static AhpProjectId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
