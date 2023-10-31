namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfStoreys : RequiredIntValueObject
{
    public NumberOfStoreys(string? value)
        : base(value, nameof(NumberOfStoreys), "How many storeys in this home type", 0, 99)
    {
    }
}
