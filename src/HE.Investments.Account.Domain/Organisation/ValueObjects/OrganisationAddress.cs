using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Utils.Constants;

namespace HE.Investments.Account.Domain.Organisation.ValueObjects;

public class OrganisationAddress : ValueObject
{
    public OrganisationAddress(
        string? addressLine1,
        string? addressLine2,
        string? addressLine3,
        string? townOrCity,
        string? postcode,
        string? county,
        string? country,
        string? lengthErrorMessage = null)
    {
        Build(addressLine1, addressLine2, addressLine3, townOrCity, postcode, county, country, lengthErrorMessage).CheckErrors();
    }

    public string AddressLine1 { get; private set; }

    public string? AddressLine2 { get; private set; }

    public string? AddressLine3 { get; private set; }

    public string TownOrCity { get; private set; }

    public Postcode Postcode { get; private set; }

    public string? County { get; private set; }

    public string? Country { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return AddressLine1;
        yield return AddressLine2;
        yield return AddressLine3;
        yield return TownOrCity;
        yield return Postcode;
        yield return County;
        yield return Country;
    }

    private OperationResult Build(
        string? line1,
        string? line2,
        string? line3,
        string? city,
        string? postcode,
        string? county,
        string? country,
        string? lengthErrorMessage)
    {
        var operationResult = OperationResult.New();
        lengthErrorMessage = lengthErrorMessage != null ? ValidationErrorMessage.ShortInputLengthExceeded(lengthErrorMessage) : null;

        AddressLine1 = Validator
            .For(line1, nameof(AddressLine1), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationAddress)
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Address Line 1"));

        AddressLine2 = Validator
            .For(line2, nameof(AddressLine2), operationResult)
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Address Line 2"));

        AddressLine3 = Validator
            .For(line3, nameof(AddressLine3), operationResult)
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Address Line 3"));

        TownOrCity = Validator
            .For(city, nameof(TownOrCity), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationTownOrCity)
            .IsShortInput(lengthErrorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded("Town or City"));

        County = Validator
            .For(county, nameof(County), operationResult)
            .IsShortInput(lengthErrorMessage);

        Country = Validator
            .For(country, nameof(Country), operationResult)
            .IsShortInput(lengthErrorMessage);

        Postcode = operationResult.Aggregate(() => new Postcode(postcode));

        return operationResult;
    }
}
