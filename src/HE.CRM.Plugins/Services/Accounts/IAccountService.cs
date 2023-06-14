using HE.Base.Services;
using Microsoft.Xrm.Sdk;
using DataverseModel;

namespace HE.CRM.Plugins.Services.Accounts
{
    public interface IAccountService : ICrmService
    {
        string GenerateRandomAccountSampleName();
    }
}
