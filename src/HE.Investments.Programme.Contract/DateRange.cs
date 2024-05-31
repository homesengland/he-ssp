namespace HE.Investments.Programme.Contract;

public record DateRange(DateOnly Start, DateOnly End)
{
    public DateTime StartDate => Start.ToDateTime(TimeOnly.MinValue);

    public DateTime EndDate => End.ToDateTime(TimeOnly.MinValue);
}
