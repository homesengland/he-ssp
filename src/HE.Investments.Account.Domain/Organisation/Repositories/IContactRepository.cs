using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IContactRepository
{
    Task LinkOrganisation(OrganisationId organisationId, int portalType);
}
