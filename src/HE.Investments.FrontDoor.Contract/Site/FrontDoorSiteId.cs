using HE.Investments.Common.Contract;

namespace HE.Investments.FrontDoor.Contract.Site;

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
