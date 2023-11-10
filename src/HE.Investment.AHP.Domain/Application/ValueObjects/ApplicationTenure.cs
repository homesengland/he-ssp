using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

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
