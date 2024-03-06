using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class OrganisationHomesBuilt : YourRequiredIntValueObject
{
    private const string DisplayName = "previous residential building experience";

    private const int MinValue = 0;

    private const int MaxValue = 9999;

    public OrganisationHomesBuilt(string? value)
        : base(value, nameof(OrganisationHomesBuilt), DisplayName, MinValue, MaxValue)
    {
    }

    public OrganisationHomesBuilt(int value)
        : base(value, nameof(OrganisationHomesBuilt), DisplayName, MinValue, MaxValue)
    {
    }
}
