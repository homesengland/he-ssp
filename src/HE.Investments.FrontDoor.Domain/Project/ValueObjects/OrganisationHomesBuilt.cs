using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investments.FrontDoor.Domain.Project.ValueObjects;

public class OrganisationHomesBuilt : YourRequiredIntValueObject, IQuestion
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

    public bool IsAnswered()
    {
        return Value.IsProvided();
    }
}
