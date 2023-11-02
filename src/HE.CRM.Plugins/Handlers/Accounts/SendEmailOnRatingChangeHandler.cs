using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.Accounts;


namespace HE.CRM.Plugins.Handlers.Accounts
{
    public class SendEmailOnRatingChangeHandler : CrmEntityHandlerBase<Account, DataverseContext>
    {
        #region Fields

        private Account target => ExecutionData.Target;
        private Account preImage => ExecutionData.PreImage;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return target != null && preImage != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IAccountService>().SendEmailOnRatingChange(target, preImage);
        }

        #endregion
    }
}
