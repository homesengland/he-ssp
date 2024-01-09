using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class Postcode : ValueObject
{
    public Postcode(string? value)
    {
        Build(value).CheckErrors();
    }

    public string Value { get; private set; }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string? value)
    {
        var operationResult = OperationResult.New();

        Value = Validator
            .For(value, nameof(Postcode), "Postcode", operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationPostCode)
            .IsShortInput()
            .IsValidPostcode(OrganisationErrorMessages.InvalidOrganisationPostcode);

        return operationResult;
    }
}
