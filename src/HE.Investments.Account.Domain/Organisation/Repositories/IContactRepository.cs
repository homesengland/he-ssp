using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IContactRepository
{
    Task LinkOrganisation(OrganisationId organisationId, int portalType);
}
