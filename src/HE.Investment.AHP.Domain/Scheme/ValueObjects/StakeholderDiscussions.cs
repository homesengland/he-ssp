using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class StakeholderDiscussions : ValueObject
{
    public StakeholderDiscussions(string? report)
    {
        Build(report).CheckErrors();
    }

    public string Report { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Report;
    }

    private OperationResult Build(string? report)
    {
        var operationResult = OperationResult.New();

        Report = Validator
            .For(report, "StakeholderDiscussionsReport", operationResult)
            .IsLongInput();

        return operationResult;
    }
}
