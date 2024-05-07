using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Shared.Project;

public record FrontDoorSiteId : StringIdValueObject
{
    public FrontDoorSiteId(string id)
        : base(id)
    {
    }

    private FrontDoorSiteId()
    {
    }

    public static FrontDoorSiteId New() => new();

    public static FrontDoorSiteId From(string value) => new(FromStringToShortGuidAsString(value));

    public string ToGuidAsString() => Value.IsProvided() ? Value.ToGuidAsString() : Value;

    public static FrontDoorSiteId? Create(string? id) => id.IsProvided() ? From(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
