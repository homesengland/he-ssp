using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

public class LandRegistryDetails : ValueObject, IQuestion
{
    public LandRegistryDetails(bool? isLandRegistryTitleNumberRegistered, LandRegistryTitleNumber? titleNumber)
    {
        IsLandRegistryTitleNumberRegistered = isLandRegistryTitleNumberRegistered;
        TitleNumber = isLandRegistryTitleNumberRegistered == true ? titleNumber : null;
    }

    public bool? IsLandRegistryTitleNumberRegistered { get; }

    public LandRegistryTitleNumber? TitleNumber { get; }

    public static LandRegistryDetails Create(bool? isLandRegistryTitleNumberRegistered, LandRegistryTitleNumber? titleNumber) =>
        new(isLandRegistryTitleNumberRegistered, titleNumber);

    public bool IsAnswered()
    {
        return IsLandRegistryTitleNumberRegistered == false || TitleNumber.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsLandRegistryTitleNumberRegistered;
        yield return TitleNumber;
    }
}
