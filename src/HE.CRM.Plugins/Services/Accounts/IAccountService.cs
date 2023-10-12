using HE.Base.Services;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Services.Accounts
{
    public interface IAccountService : ICrmService
    {
        string GenerateRandomAccountSampleName();
        OrganizationDetailsDto GetOrganizationDetails(string accountid, string contactExternalId);
        void OnCurrentCrrFieldUpdate(Account target, Account preImage);
        string GetOrganisationChangeDetails(string accountId);
    }
}
