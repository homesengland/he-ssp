using HE.InvestmentLoans.BusinessLogic.Organization;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationName : ValueObject
{
    public ApplicationName(string value)
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
