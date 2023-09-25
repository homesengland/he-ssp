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
    "invln_currentcrr",
    "HE.CRM.Plugins.Plugins.Account.OnCurrentCrrFieldUpdatePlugin: Update of Account",
    1,
    IsolationModeEnum.Sandbox,
    Id = "f3644d70-dafb-4a99-ace9-4a3e7f67f1c3",
    Image1Name = "PreImage", Image1Attributes = "",
    Image1Type = ImageTypeEnum.PreImage)]
    public class OnCurrentCrrFieldUpdatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public OnCurrentCrrFieldUpdatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<OnCurrentCrrFieldUpdateHandler>());
        }
        #endregion
    }
}
