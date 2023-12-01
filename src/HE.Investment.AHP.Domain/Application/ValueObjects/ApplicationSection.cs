using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationSection : ValueObject
{
    public ApplicationSection(SectionType type, SectionStatus status = SectionStatus.NotStarted)
    {
        Type = type;
        Status = status;
    }

    public SectionType Type { get; }

    public SectionStatus Status { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Status;
        yield return Type;
    }
}
