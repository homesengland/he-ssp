using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeName : ValueObject
{
    public SchemeName(string value)
    {
        Build(value).CheckErrors();
    }

    public string Name { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
    }

    private OperationResult Build(string name)
    {
        var operationResult = OperationResult.New();

        Name = Validator
            .For(name, nameof(Name), operationResult)
            .IsProvided()
            .IsShortInput();

        return operationResult;
    }
}
