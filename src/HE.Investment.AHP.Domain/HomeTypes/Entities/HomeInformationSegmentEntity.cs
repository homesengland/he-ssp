using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.HomeInformation)]
public class HomeInformationSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public HomeInformationSegmentEntity(
        NumberOfHomes? numberOfHomes = null,
        NumberOfBedrooms? numberOfBedrooms = null,
        MaximumOccupancy? maximumOccupancy = null,
        NumberOfStoreys? numberOfStoreys = null,
        YesNoType intendedAsMoveOnAccommodation = YesNoType.Undefined)
    {
        NumberOfHomes = numberOfHomes;
        NumberOfBedrooms = numberOfBedrooms;
        MaximumOccupancy = maximumOccupancy;
        NumberOfStoreys = numberOfStoreys;
        IntendedAsMoveOnAccommodation = intendedAsMoveOnAccommodation;
    }

    public bool IsModified => _modificationTracker.IsModified;

    public NumberOfHomes? NumberOfHomes { get; private set; }

    public NumberOfBedrooms? NumberOfBedrooms { get; private set; }

    public MaximumOccupancy? MaximumOccupancy { get; private set; }

    public NumberOfStoreys? NumberOfStoreys { get; private set; }

    public YesNoType IntendedAsMoveOnAccommodation { get; private set; }

    public void ChangeNumberOfHomes(string? numberOfHomes)
    {
        var newValue = numberOfHomes.IsProvided() ? new NumberOfHomes(numberOfHomes) : null;
        NumberOfHomes = _modificationTracker.Change(NumberOfHomes, newValue);
    }

    public void ChangeNumberOfBedrooms(string? numberOfBedrooms)
    {
        var newValue = numberOfBedrooms.IsProvided() ? new NumberOfBedrooms(numberOfBedrooms) : null;
        NumberOfBedrooms = _modificationTracker.Change(NumberOfBedrooms, newValue);
    }

    public void ChangeMaximumOccupancy(string? maximumOccupancy)
    {
        var newValue = maximumOccupancy.IsProvided() ? new MaximumOccupancy(maximumOccupancy) : null;
        MaximumOccupancy = _modificationTracker.Change(MaximumOccupancy, newValue);
    }

    public void ChangeNumberOfStoreys(string? numberOfStoreys)
    {
        var newValue = numberOfStoreys.IsProvided() ? new NumberOfStoreys(numberOfStoreys) : null;
        NumberOfStoreys = _modificationTracker.Change(NumberOfStoreys, newValue);
    }

    public void ChangeIntendedAsMoveOnAccommodation(YesNoType intendedAsMoveOnAccommodation)
    {
        IntendedAsMoveOnAccommodation = _modificationTracker.Change(IntendedAsMoveOnAccommodation, intendedAsMoveOnAccommodation);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new HomeInformationSegmentEntity(NumberOfHomes, NumberOfBedrooms, MaximumOccupancy, NumberOfStoreys);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted()
    {
        return NumberOfHomes.IsProvided()
               && NumberOfBedrooms.IsProvided()
               && MaximumOccupancy.IsProvided()
               && NumberOfStoreys.IsProvided();
    }
}
