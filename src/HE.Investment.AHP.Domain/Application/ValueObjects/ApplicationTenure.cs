using HE.Investment.AHP.Domain.Application.Entities;
using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationTenure : ValueObject
{
    public ApplicationTenure(string value)
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
