using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeName : ValueObject
{
    public string Name { get; private set; }

    public SchemeName(string value)
    {
        Build(value).CheckErrors();
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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
    }
}
