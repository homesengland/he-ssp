using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class AuditEntry : ValueObject
{
    public AuditEntry(string? firstName, string? lastName, DateTime? changedOn)
    {
        FirstName = firstName;
        LastName = lastName;
        ChangedOn = changedOn;
    }

    public string? FirstName { get; }

    public string? LastName { get; }

    public DateTime? ChangedOn { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
        yield return ChangedOn;
    }
}
