using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.HomeInformation)]
public class HomeInformationSegmentEntity : IHomeTypeSegmentEntity
{
    public HomeInformationSegmentEntity(
        NumberOfHomes? numberOfHomes = null,
        NumberOfBedrooms? numberOfBedrooms = null,
        MaximumOccupancy? maximumOccupancy = null,
        NumberOfStoreys? numberOfStoreys = null)
    {
        NumberOfHomes = numberOfHomes;
        NumberOfBedrooms = numberOfBedrooms;
        MaximumOccupancy = maximumOccupancy;
        NumberOfStoreys = numberOfStoreys;
    }

    public NumberOfHomes? NumberOfHomes { get; private set; }

    public NumberOfBedrooms? NumberOfBedrooms { get; private set; }

    public MaximumOccupancy? MaximumOccupancy { get; private set; }

    public NumberOfStoreys? NumberOfStoreys { get; private set; }

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

    public bool IsCompleted()
    {
        return NumberOfHomes.IsProvided()
               && NumberOfBedrooms.IsProvided()
               && MaximumOccupancy.IsProvided()
               && NumberOfStoreys.IsProvided();
    }
}
