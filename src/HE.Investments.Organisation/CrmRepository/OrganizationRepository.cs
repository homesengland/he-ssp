using HE.Investments.Organisation.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace HE.Investments.Organisation.CrmRepository;
public class OrganizationRepository : IOrganizationRepository
{
    public IEnumerable<Account> GetAccountsByOrganizationNameAndCompanyHouseNumber(IOrganizationService service, string organizationName, string companyHouseNumber)
    {
        using var ctx = new OrganizationServiceContext(service);
        return ctx.CreateQuery<Account>()
            .Where(x => x.Name.Contains(organizationName) || x.he_CompaniesHouseNumber == companyHouseNumber).AsEnumerable();
    }
}
