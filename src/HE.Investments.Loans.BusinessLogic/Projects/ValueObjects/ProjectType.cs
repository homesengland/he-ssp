using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class ProjectType : ValueObject
{
    public ProjectType(string? value)
    {
        Value = value;
    }

    public static ProjectType Default => new(string.Empty);

    public string? Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value ?? null!;
    }
}
