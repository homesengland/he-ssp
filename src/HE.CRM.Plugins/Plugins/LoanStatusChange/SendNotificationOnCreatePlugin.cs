using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.LoanStatusChange;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.LoanApplications
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_Loanstatuschange.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.LoanStatusChange.SendNotificationOnCreatePlugin: Create of Loan Status Change",
       1,
       IsolationModeEnum.Sandbox,
       Id = "1e78f173-c44a-41d5-8cad-287985591a2c")]
    public class SendNotificationOnCreatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendNotificationOnCreatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendNotificationOnCreateHandler>());
        }
        #endregion
    }
}
