using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class ProjectName : YourShortText
{
    public ProjectName(string value)
        : base(value, "Name", "project name")
    {
    }
}
