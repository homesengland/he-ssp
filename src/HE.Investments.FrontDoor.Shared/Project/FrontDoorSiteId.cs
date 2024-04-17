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

    public static FrontDoorSiteId? Create(string? id) => id.IsProvided() ? new FrontDoorSiteId(id!) : null;

    public override string ToString()
    {
        return Value;
    }
}
