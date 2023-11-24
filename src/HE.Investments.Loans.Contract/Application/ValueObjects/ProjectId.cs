using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;
public class ProjectId : ValueObject
{
    public ProjectId(Guid value)
    {
        Value = Guard.Argument(value, nameof(ProjectId)).NotDefault();
    }

    public Guid Value { get; }

    public static ProjectId From(Guid value) => new(value);

    public static ProjectId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
