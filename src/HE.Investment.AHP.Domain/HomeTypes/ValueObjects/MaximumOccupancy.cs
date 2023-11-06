namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MaximumOccupancy : RequiredIntValueObject
{
    public MaximumOccupancy(string? value)
        : base(value, nameof(MaximumOccupancy), "The maximum number of people who can live in this home", 0, 999)
    {
    }
}
