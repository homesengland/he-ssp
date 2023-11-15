namespace HE.Investments.Account.Domain.Organisation.Repositories;

public interface IContactRepository
{
    Task LinkOrganisation(string organisationId, string portalType);
}
