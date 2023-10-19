using System.Text.Json.Serialization;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class Postcode : ValueObject
{
    public Postcode(string value)
    {
        Build(value).CheckErrors();
    }

    public string Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    private OperationResult Build(string value)
    {
        var operationResult = OperationResult.New();

        Value = Validator
            .For(value, nameof(Postcode), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationPostCode)
            .IsShortInput()
            .IsValidPostcode(OrganisationErrorMessages.InvalidOrganisationPostcode);

        return operationResult;
    }
}
