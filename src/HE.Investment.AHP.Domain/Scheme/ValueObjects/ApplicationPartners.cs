extern alias Org;

using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class ApplicationPartners : ValueObject, IQuestion
{
    public ApplicationPartners(
        InvestmentsOrganisation developingPartner,
        InvestmentsOrganisation ownerOfTheLand,
        InvestmentsOrganisation ownerOfTheHomes,
        bool? isConfirmed = null)
    {
        DevelopingPartner = developingPartner;
        OwnerOfTheLand = ownerOfTheLand;
        OwnerOfTheHomes = ownerOfTheHomes;
        IsConfirmed = isConfirmed;
    }

    public InvestmentsOrganisation DevelopingPartner { get; }

    public InvestmentsOrganisation OwnerOfTheLand { get; }

    public InvestmentsOrganisation OwnerOfTheHomes { get; }

    public bool? IsConfirmed { get; }

    public static ApplicationPartners ConfirmedPartner(OrganisationBasicInfo organisation)
    {
        var partner = new InvestmentsOrganisation(organisation.OrganisationId, organisation.RegisteredCompanyName);

        return new ApplicationPartners(partner, partner, partner, true);
    }

    public static ApplicationPartners ConfirmedPartner(OrganisationId organisationId)
    {
        return ConfirmedPartner(new OrganisationBasicInfo(organisationId, string.Empty, string.Empty, string.Empty, false));
    }

    public static ApplicationPartners FromSitePartners(SitePartners sitePartners)
    {
        if (!sitePartners.IsAnswered())
        {
            throw new DomainValidationException("Cannot create Application Partners because Site Partners section is not completed.");
        }

        return new ApplicationPartners(sitePartners.DevelopingPartner!, sitePartners.OwnerOfTheLand!, sitePartners.OwnerOfTheHomes!);
    }

    public ApplicationPartners WithDevelopingPartner(InvestmentsOrganisation developingPartner)
    {
        return new ApplicationPartners(developingPartner, OwnerOfTheLand, OwnerOfTheHomes, developingPartner == DevelopingPartner ? IsConfirmed : null);
    }

    public ApplicationPartners WithOwnerOfTheLand(InvestmentsOrganisation ownerOfTheLand)
    {
        return new ApplicationPartners(DevelopingPartner, ownerOfTheLand, OwnerOfTheHomes, ownerOfTheLand == OwnerOfTheLand ? IsConfirmed : null);
    }

    public ApplicationPartners WithOwnerOfTheHomes(InvestmentsOrganisation ownerOfTheHomes)
    {
        return new ApplicationPartners(DevelopingPartner, OwnerOfTheLand, ownerOfTheHomes, ownerOfTheHomes == OwnerOfTheHomes ? IsConfirmed : null);
    }

    public ApplicationPartners WithConfirmation(bool? isConfirmed)
    {
        return new ApplicationPartners(DevelopingPartner, OwnerOfTheLand, OwnerOfTheHomes, isConfirmed);
    }

    public bool IsAnswered()
    {
        return IsConfirmed == true;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return DevelopingPartner.Id;
        yield return OwnerOfTheLand.Id;
        yield return OwnerOfTheHomes.Id;
        yield return IsConfirmed;
    }
}
