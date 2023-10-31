using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeFunding : ValueObject
{
    public SchemeFunding(decimal? requiredFunding, int? housesToDeliver)
    {
        Build(requiredFunding, housesToDeliver).CheckErrors();
    }

    public decimal RequiredFunding { get; private set; }

    public int HousesToDeliver { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RequiredFunding;
        yield return HousesToDeliver;
    }

    private OperationResult Build(decimal? requiredFunding, int? housesToDeliver)
    {
        var operationResult = OperationResult.New();

        RequiredFunding = NumericValidator
            .For(requiredFunding, nameof(RequiredFunding), operationResult)
            .IsProvided()
            .IsNotDefault()
            .IsPositive();

        HousesToDeliver = NumericValidator
            .For(housesToDeliver, nameof(HousesToDeliver), operationResult)
            .IsProvided()
            .IsNotDefault()
            .IsPositive();

        return operationResult;
    }
}
