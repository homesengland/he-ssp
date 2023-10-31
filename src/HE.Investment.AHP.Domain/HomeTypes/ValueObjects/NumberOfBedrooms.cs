namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfBedrooms : RequiredIntValueObject
{
    public NumberOfBedrooms(string? value)
        : base(value, nameof(NumberOfBedrooms), "How many bedrooms in each home", 0, 999)
    {
    }
}
