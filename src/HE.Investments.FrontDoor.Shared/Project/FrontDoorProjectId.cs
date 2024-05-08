using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Shared.Project;

public record FrontDoorProjectId : StringIdValueObject
{
    public FrontDoorProjectId(string value)
        : base(value)
    {
    }

    private FrontDoorProjectId()
    {
    }

    public static FrontDoorProjectId New() => new();

    public static FrontDoorProjectId From(string value) => new(FromStringToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public static FrontDoorProjectId? Create(string? id) => id.IsProvided() ? From(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
