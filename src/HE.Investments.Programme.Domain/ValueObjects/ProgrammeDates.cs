using HE.Investments.Common.Domain;

namespace HE.Investments.Programme.Domain.ValueObjects;

public class ProgrammeDates : ValueObject
{
    public ProgrammeDates(DateOnly? start, DateOnly? end)
    {
        Start = start ?? DateOnly.MinValue;
        End = end ?? DateOnly.MaxValue;
    }

    public DateOnly Start { get; }

    public DateOnly End { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Start;
        yield return End;
    }
}
