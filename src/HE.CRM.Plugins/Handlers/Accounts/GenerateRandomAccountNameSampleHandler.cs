using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;

namespace HE.CRM.Plugins.Handlers.Accounts
{
    public class GenerateRandomAccountNameSampleHandler : CrmEntityHandlerBase<Account, DataverseContext>
    {
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return true;
        }

        public override void DoWork()
        {
            var randomAccountName = CrmServicesFactory.Get<IAccountService>().GenerateRandomAccountSampleName();
            ExecutionData.Target.Name = randomAccountName;
        }
        #endregion
    }
}
