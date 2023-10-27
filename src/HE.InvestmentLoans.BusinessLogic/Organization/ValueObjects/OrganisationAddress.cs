using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization.ValueObjects;

public class OrganisationAddress : ValueObject
{
    public OrganisationAddress(
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string townOrCity,
        string postcode,
        string? county,
        string? country)
    {
        Build(addressLine1, addressLine2, addressLine3, townOrCity, postcode, county, country).CheckErrors();
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

    private OperationResult Build(string line1, string? line2, string? line3, string city, string postcode, string? county, string? country)
    {
        var operationResult = OperationResult.New();

        AddressLine1 = Validator
            .For(line1, nameof(AddressLine1), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationAddress)
            .IsShortInput();

        AddressLine2 = Validator
            .For(line2, nameof(AddressLine2), operationResult)
            .IsShortInput();

        AddressLine3 = Validator
            .For(line3, nameof(AddressLine3), operationResult)
            .IsShortInput();

        TownOrCity = Validator
            .For(city, nameof(TownOrCity), operationResult)
            .IsProvided(OrganisationErrorMessages.MissingOrganisationTownOrCity)
            .IsShortInput();

        County = Validator
            .For(county, nameof(County), operationResult)
            .IsShortInput();

        Country = Validator
            .For(country, nameof(Country), operationResult)
            .IsShortInput();

        Postcode = operationResult.Aggregate(() => new Postcode(postcode));

        return operationResult;
    }
}
