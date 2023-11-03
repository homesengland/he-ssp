using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.Plugins.Handlers.Accounts;

namespace HE.CRM.Plugins.Plugins.Accounts
{
    [CrmPluginRegistration(
    MessageNameEnum.Update,
    Account.EntityLogicalName,
    StageEnum.PreOperation,
    ExecutionModeEnum.Synchronous,
    "invln_rating",
    "HE.CRM.Plugins.Plugins.Account.OnCurrentCrrFieldUpdatePlugin: Update of Account",
    1,
    IsolationModeEnum.Sandbox,
    Id = "26603b78-b7b8-451e-872c-2693afefcdc1",
    Image1Name = "PreImage", Image1Attributes = "",
    Image1Type = ImageTypeEnum.PreImage)]
    public class SendEmailOnRatingChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendEmailOnRatingChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendEmailOnRatingChangeHandler>());
        }
        #endregion
    }
}
