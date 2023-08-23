using HE.InvestmentLoans.CRM.Model;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganizationRepository
{
    IEnumerable<Account> GetAccountsByOrganizationNameAndCompanyHouseNumber(IOrganizationService service, string organizationName, string companyHouseNumber);
}
