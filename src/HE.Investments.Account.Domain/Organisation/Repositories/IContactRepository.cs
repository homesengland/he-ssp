using HE.Investments.Account.Domain.Organisation.ValueObjects;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IContactRepository
{
    Task LinkOrganisation(OrganisationId organisationId, int portalType);
}
