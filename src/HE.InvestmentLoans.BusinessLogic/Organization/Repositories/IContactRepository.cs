namespace HE.InvestmentLoans.BusinessLogic.Organization.Repositories;

public interface IContactRepository
{
    Task LinkOrganisation(string organisationId, string portalType);
}
