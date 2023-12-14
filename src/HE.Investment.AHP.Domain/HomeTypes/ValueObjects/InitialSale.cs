namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class InitialSale : RequiredIntForCalculationValueObject
{
    private const string DisplayName = "initial sale";

    private const int MinValue = 10;

    private const int MaxValue = 75;

    public InitialSale(string? value, bool isCalculation)
        : base(value, nameof(InitialSale), DisplayName, MinValue, MaxValue, isCalculation)
    {
    }

    public InitialSale(int value)
        : base(value, nameof(InitialSale), DisplayName, MinValue, MaxValue)
    {
    }
}
