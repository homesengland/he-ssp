using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

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
            .For(name, nameof(Name), "Application name", operationResult)
            .IsProvided()
            .IsShortInput();

        return operationResult;
    }
}
