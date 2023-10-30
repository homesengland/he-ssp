namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfHomes : RequiredIntValueObject
{
    public NumberOfHomes(string? value)
        : base(value, nameof(NumberOfHomes), "How many homes in this home type", 0, 999)
    {
    }
}
