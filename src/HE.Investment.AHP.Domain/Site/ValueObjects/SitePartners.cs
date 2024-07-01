using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class SitePartners : ValueObject, IQuestion
{
    public SitePartners(
        InvestmentsOrganisation? developingPartner = null,
        InvestmentsOrganisation? ownerOfTheLand = null,
        InvestmentsOrganisation? ownerOfTheHomes = null)
    {
        DevelopingPartner = developingPartner;
        OwnerOfTheLand = ownerOfTheLand;
        OwnerOfTheHomes = ownerOfTheHomes;
    }

    public InvestmentsOrganisation? DevelopingPartner { get; }

    public InvestmentsOrganisation? OwnerOfTheLand { get; }

    public InvestmentsOrganisation? OwnerOfTheHomes { get; }

    public static SitePartners SinglePartner(OrganisationBasicInfo organisation)
    {
        var partner = new InvestmentsOrganisation(organisation.OrganisationId, organisation.RegisteredCompanyName);

        return new SitePartners(partner, partner, partner);
    }

    public SitePartners WithDevelopingPartner(InvestmentsOrganisation developingPartner, bool? isConfirmed)
    {
        return Create(developingPartner, OwnerOfTheLand, OwnerOfTheHomes, isConfirmed, "developing partner");
    }

    public SitePartners WithOwnerOfTheLand(InvestmentsOrganisation ownerOfTheLand, bool? isConfirmed)
    {
        return Create(DevelopingPartner, ownerOfTheLand, OwnerOfTheHomes, isConfirmed, "owner of the land during development");
    }

    public SitePartners WithOwnerOfTheHomes(InvestmentsOrganisation ownerOfTheHomes, bool? isConfirmed)
    {
        return Create(DevelopingPartner, OwnerOfTheLand, ownerOfTheHomes, isConfirmed, "owner of the homes after completion");
    }

    public bool IsAnswered()
    {
        return DevelopingPartner.IsProvided() && OwnerOfTheLand.IsProvided() && OwnerOfTheHomes.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return DevelopingPartner?.Id;
        yield return OwnerOfTheLand?.Id;
        yield return OwnerOfTheHomes?.Id;
    }

    private SitePartners Create(
        InvestmentsOrganisation? developingPartner,
        InvestmentsOrganisation? ownerOfTheLand,
        InvestmentsOrganisation? ownerOfTheHomes,
        bool? isConfirmed,
        string partnerType)
    {
        if (isConfirmed == null)
        {
            OperationResult.ThrowValidationError(nameof(isConfirmed), $"Select yes to confirm the {partnerType}");
        }

        return isConfirmed!.Value
            ? new SitePartners(developingPartner, ownerOfTheLand, ownerOfTheHomes)
            : this;
    }
}
