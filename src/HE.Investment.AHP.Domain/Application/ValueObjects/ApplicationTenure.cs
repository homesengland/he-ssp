using HE.Investment.AHP.Domain.Application.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationTenure : ValueObject
{
    public ApplicationTenure(Tenure value)
    {
        Build(value).CheckErrors();
    }

    public Tenure Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(Tenure value)
    {
        var operationResult = OperationResult.New();

        Value = EnumValidator<Tenure>.Required(value, nameof(Tenure), operationResult);

        return operationResult;
    }
}
