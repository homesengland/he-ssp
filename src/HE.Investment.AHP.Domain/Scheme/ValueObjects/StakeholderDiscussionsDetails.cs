using HE.Investment.AHP.Domain.Scheme.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class StakeholderDiscussionsDetails : ValueObject
{
    public StakeholderDiscussionsDetails(string? report)
    {
        Build(report).CheckErrors();
    }

    public string? Report { get; private set; }

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
            .For(report, "StakeholderDiscussionsReport", "Local stakeholder discussions", operationResult)
            .IsProvidedIf(isCompleteCheck)
            .IsLongInput();

        return operationResult;
    }
}
