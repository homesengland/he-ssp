using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investments.Organisation.Entities;

public class ManualOrganisationEntity : IManualOrganisation
{
    private ManualOrganisationEntity(
        ShortText name,
        ShortText addressLine1,
        ShortText? addressLine2,
        ShortText townOrCity,
        ShortText? county,
        Postcode postcode)
    {
        Name = name;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        TownOrCity = townOrCity;
        County = county;
        Postcode = postcode;
    }

    public ShortText Name { get; }

    public ShortText AddressLine1 { get; }

    public ShortText? AddressLine2 { get; }

    public ShortText TownOrCity { get; }

    public ShortText? County { get; }

    public ShortText Postcode { get; }

    public static ManualOrganisationEntity Create(
        string? organisationName,
        string? organisationAddressLine1,
        string? organisationAddressLine2,
        string? organisationTownOrCity,
        string? organisationCounty,
        string? organisationPostcode)
    {
        var operationResult = new OperationResult();

        var name = operationResult.AggregateNullable(() => new AddressPart(organisationName, nameof(Name), "name"));
        var addressLine1 = operationResult.AggregateNullable(
            () => new AddressPart(organisationAddressLine1, nameof(AddressLine1), "address line 1"));
        var addressLine2 = operationResult.AggregateNullable(() =>
            string.IsNullOrWhiteSpace(organisationAddressLine2) ? null : new AddressPart(organisationAddressLine2, nameof(AddressLine2), "address line 2"));
        var townOrCity = operationResult.AggregateNullable(() => new AddressPart(organisationTownOrCity, nameof(TownOrCity), "town or city"));
        var county = operationResult.AggregateNullable(
            () => string.IsNullOrWhiteSpace(organisationCounty) ? null : new AddressPart(organisationCounty, nameof(County), "county"));
        var postcode = operationResult.AggregateNullable(() => new Postcode(organisationPostcode));

        operationResult.CheckErrors();

        return new ManualOrganisationEntity(name!, addressLine1!, addressLine2, townOrCity!, county, postcode!);
    }

    private sealed class AddressPart(string? value, string fieldName, string fieldDisplayName)
        : ShortText(value, fieldName, $"organisation {fieldDisplayName}");
}
