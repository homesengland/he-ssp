using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class StakeholderDiscussions : ValueObject
{
    public StakeholderDiscussions(string? report)
    {
        Build(report).CheckErrors();
    }

    public string Report { get; private set; }

    public void CheckIsComplete()
    {
        Build(Report, true).CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Report;
    }

    private OperationResult Build(string? report, bool isCompleteCheck = false)
    {
        var operationResult = OperationResult.New();

        Report = Validator
            .For(report, "StakeholderDiscussionsReport", operationResult)
            .IsProvidedIf(isCompleteCheck, "Local stakeholder discussions are missing")
            .IsLongInput();

        return operationResult;
    }
}
