using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
public class LoanApplicationSection : ValueObject
{
    public LoanApplicationSection(SectionStatus status)
    {
        Status = status;
    }

    public SectionStatus Status { get; }

    public static LoanApplicationSection New() => new(SectionStatus.NotStarted);

    public bool IsCompleted() => Status == SectionStatus.Completed;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Status;
    }
}
