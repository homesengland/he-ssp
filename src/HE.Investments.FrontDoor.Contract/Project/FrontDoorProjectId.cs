using HE.Investments.Common.Contract;

namespace HE.Investments.FrontDoor.Contract.Project;

public record FrontDoorProjectId : StringIdValueObject
{
    public FrontDoorProjectId(string id)
        : base(id)
    {
    }

    private FrontDoorProjectId()
    {
    }

    public static FrontDoorProjectId New() => new();

    public override string ToString()
    {
        return Value;
    }
}
