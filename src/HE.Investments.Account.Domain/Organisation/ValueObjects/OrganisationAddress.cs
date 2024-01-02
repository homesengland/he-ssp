using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

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
        string? fieldName = null)
    {
        Build(addressLine1, addressLine2, addressLine3, townOrCity, postcode, county, country, fieldName).CheckErrors();
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
        string? fieldName)
    {
        var operationResult = OperationResult.New();
        var errorMessage = fieldName != null ? ValidationErrorMessage.ShortInputLengthExceeded(fieldName) : null;

        AddressLine1 = Validator
            .For(line1, nameof(AddressLine1), "Address Line 1", operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationAddress)
            .IsShortInput(errorMessage);

        AddressLine2 = Validator
            .For(line2, nameof(AddressLine2), "Address Line 2", operationResult)
            .IsShortInput(errorMessage);

        AddressLine3 = Validator
            .For(line3, nameof(AddressLine3), "Address Line 3", operationResult)
            .IsShortInput(errorMessage);

        TownOrCity = Validator
            .For(city, nameof(TownOrCity), "Town or City", operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationTownOrCity)
            .IsShortInput(errorMessage);

        County = Validator
            .For(county, nameof(County), "County", operationResult)
            .IsShortInput(errorMessage);

        Country = Validator
            .For(country, nameof(Country), "Country", operationResult)
            .IsShortInput(errorMessage);

        Postcode = operationResult.Aggregate(() => new Postcode(postcode));

        return operationResult;
    }
}
