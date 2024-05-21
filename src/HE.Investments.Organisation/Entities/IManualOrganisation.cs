using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Organisation.Entities;

public interface IManualOrganisation
{
    public ShortText Name { get; }

    public ShortText AddressLine1 { get; }

    public ShortText? AddressLine2 { get; }

    public ShortText TownOrCity { get; }

    public ShortText? County { get; }

    public ShortText Postcode { get; }
}
