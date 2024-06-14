using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class ApplicationPartners : ValueObject
{
    public ApplicationPartners(
        InvestmentsOrganisation developingPartner,
        InvestmentsOrganisation ownerOfTheLand,
        InvestmentsOrganisation ownerOfTheHomes,
        bool? arePartnersConfirmed = null)
    {
        DevelopingPartner = developingPartner;
        OwnerOfTheLand = ownerOfTheLand;
        OwnerOfTheHomes = ownerOfTheHomes;
        ArePartnersConfirmed = arePartnersConfirmed;
    }

    public InvestmentsOrganisation DevelopingPartner { get; }

    public InvestmentsOrganisation OwnerOfTheLand { get; }

    public InvestmentsOrganisation OwnerOfTheHomes { get; }

    public bool? ArePartnersConfirmed { get; }

    public static ApplicationPartners ConfirmedPartner(OrganisationBasicInfo organisation)
    {
        var partner = new InvestmentsOrganisation(organisation.OrganisationId, organisation.RegisteredCompanyName);

        return new ApplicationPartners(partner, partner, partner, true);
    }

    public static ApplicationPartners ConfirmedPartner(OrganisationId organisationId)
    {
        return ConfirmedPartner(new OrganisationBasicInfo(
            organisationId,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            false));
    }

    public static ApplicationPartners FromSitePartners(SitePartners sitePartners)
    {
        if (!sitePartners.IsAnswered())
        {
            throw new DomainValidationException("Cannot create Application Partners because Site Partners section is not completed.");
        }

        return new ApplicationPartners(sitePartners.DevelopingPartner!, sitePartners.OwnerOfTheLand!, sitePartners.OwnerOfTheHomes!);
    }

    public ApplicationPartners WithDevelopingPartner(InvestmentsOrganisation developingPartner, bool? isPartnerConfirmed)
    {
        return Create(developingPartner, OwnerOfTheLand, OwnerOfTheHomes, isPartnerConfirmed, "developing partner");
    }

    public ApplicationPartners WithOwnerOfTheLand(InvestmentsOrganisation ownerOfTheLand, bool? isPartnerConfirmed)
    {
        return Create(DevelopingPartner, ownerOfTheLand, OwnerOfTheHomes, isPartnerConfirmed, "owner of the land during development");
    }

    public ApplicationPartners WithOwnerOfTheHomes(InvestmentsOrganisation ownerOfTheHomes, bool? isPartnerConfirmed)
    {
        return Create(DevelopingPartner, OwnerOfTheLand, ownerOfTheHomes, isPartnerConfirmed, "owner of the homes after completion");
    }

    public ApplicationPartners WithPartnersConfirmation(bool? arePartnersConfirmed)
    {
        return new ApplicationPartners(DevelopingPartner, OwnerOfTheLand, OwnerOfTheHomes, arePartnersConfirmed);
    }

    public void CheckIsComplete()
    {
        if (ArePartnersConfirmed == true)
        {
            return;
        }

        OperationResult.ThrowValidationError(nameof(ArePartnersConfirmed), "Partner Details are not confirmed.");
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return DevelopingPartner.Id;
        yield return OwnerOfTheLand.Id;
        yield return OwnerOfTheHomes.Id;
        yield return ArePartnersConfirmed;
    }

    private ApplicationPartners Create(
        InvestmentsOrganisation developingPartner,
        InvestmentsOrganisation ownerOfTheLand,
        InvestmentsOrganisation ownerOfTheHomes,
        bool? isPartnerConfirmed,
        string partnerType)
    {
        if (isPartnerConfirmed == null)
        {
            OperationResult.ThrowValidationError(nameof(isPartnerConfirmed), $"Select yes if you want to confirm the {partnerType}");
        }

        if (isPartnerConfirmed!.Value)
        {
            var arePartnersTheSame = developingPartner.Id == DevelopingPartner.Id
                                     && ownerOfTheLand.Id == OwnerOfTheLand.Id
                                     && ownerOfTheHomes.Id == OwnerOfTheHomes.Id;

            return new ApplicationPartners(
                developingPartner,
                ownerOfTheLand,
                ownerOfTheHomes,
                arePartnersTheSame ? ArePartnersConfirmed : null);
        }

        return this;
    }
}
