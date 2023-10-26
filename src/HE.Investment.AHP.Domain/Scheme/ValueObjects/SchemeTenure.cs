using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeTenure : ValueObject
{
    public SchemeTenure(string value)
    {
        Build(value).CheckErrors();
    }

    public Tenure Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string value)
    {
        var operationResult = OperationResult.New();

        Value = EnumValidator<Tenure>.For(value, nameof(Tenure), operationResult);

        return operationResult;
    }
}
