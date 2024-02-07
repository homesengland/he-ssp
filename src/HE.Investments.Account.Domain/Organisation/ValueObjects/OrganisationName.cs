using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationName : ValueObject
{
    public OrganisationName(
        string? name,
        string notProvidedErrorMessage = OrganisationErrorMessages.MissingOrganisationName)
    {
        Build(name, notProvidedErrorMessage).CheckErrors();
    }

    public string Name { get; private set; }

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Name;
    }

    private OperationResult Build(string? name, string? notProvidedErrorMessage)
    {
        var operationResult = OperationResult.New();
        Name = Validator
            .For(name, nameof(Name), "Organisation name", operationResult)
            .IsProvided(notProvidedErrorMessage)
            .IsShortInput();

        return operationResult;
    }
}
