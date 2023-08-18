using HE.Base.Services;
using Microsoft.Xrm.Sdk;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Services.Accounts
{
    public interface IAccountService : ICrmService
    {
        string GenerateRandomAccountSampleName();
        OrganizationDetailsDto GetOrganizationDetails(string accountid, string contactExternalId);
        void OnCurrentCrrFieldUpdate(Account target, Account preImage);
        string SearchOrganizationByNameAndCompanyHouseName(string organizationName, string companyHouseName);
    }
}
