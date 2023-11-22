using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.HomeInformation)]
public class HomeInformationSegmentEntity : IHomeTypeSegmentEntity
{
    private NumberOfHomes? _numberOfHomes;

    private NumberOfBedrooms? _numberOfBedrooms;

    private MaximumOccupancy? _maximumOccupancy;

    private NumberOfStoreys? _numberOfStoreys;

    public HomeInformationSegmentEntity(
        NumberOfHomes? numberOfHomes = null,
        NumberOfBedrooms? numberOfBedrooms = null,
        MaximumOccupancy? maximumOccupancy = null,
        NumberOfStoreys? numberOfStoreys = null)
    {
        _numberOfHomes = numberOfHomes;
        _numberOfBedrooms = numberOfBedrooms;
        _maximumOccupancy = maximumOccupancy;
        _numberOfStoreys = numberOfStoreys;
    }

    public bool IsModified { get; private set; }

    public NumberOfHomes? NumberOfHomes
    {
        get => _numberOfHomes;
        private set
        {
            if (_numberOfHomes != value)
            {
                IsModified = true;
            }

            _numberOfHomes = value;
        }
    }

    public NumberOfBedrooms? NumberOfBedrooms
    {
        get => _numberOfBedrooms;
        private set
        {
            if (_numberOfBedrooms != value)
            {
                IsModified = true;
            }

            _numberOfBedrooms = value;
        }
    }

    public MaximumOccupancy? MaximumOccupancy
    {
        get => _maximumOccupancy;
        private set
        {
            if (_maximumOccupancy != value)
            {
                IsModified = true;
            }

            _maximumOccupancy = value;
        }
    }

    public NumberOfStoreys? NumberOfStoreys
    {
        get => _numberOfStoreys;
        private set
        {
            if (_numberOfStoreys != value)
            {
                IsModified = true;
            }

            _numberOfStoreys = value;
        }
    }

    public void ChangeNumberOfHomes(string? numberOfHomes)
    {
        NumberOfHomes = numberOfHomes.IsProvided()
            ? new NumberOfHomes(numberOfHomes)
            : null;
    }

    public void ChangeNumberOfBedrooms(string? numberOfBedrooms)
    {
        NumberOfBedrooms = numberOfBedrooms.IsProvided()
            ? new NumberOfBedrooms(numberOfBedrooms)
            : null;
    }

    public void ChangeMaximumOccupancy(string? maximumOccupancy)
    {
        MaximumOccupancy = maximumOccupancy.IsProvided()
            ? new MaximumOccupancy(maximumOccupancy)
            : null;
    }

    public void ChangeNumberOfStoreys(string? numberOfStoreys)
    {
        NumberOfStoreys = numberOfStoreys.IsProvided()
            ? new NumberOfStoreys(numberOfStoreys)
            : null;
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
