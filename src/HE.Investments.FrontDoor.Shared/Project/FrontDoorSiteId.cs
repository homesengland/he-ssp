using HE.Investments.Common.Contract;

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

    public override string ToString()
    {
        return Value;
    }
}
