using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using DataverseModel;
using HE.CRM.AHP.Plugins.Handlers.AhpStatusChange;

namespace HE.CRM.AHP.Plugins.Plugins.AhpStatusChange
{
    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_AHPStatusChange.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.AHP.Plugins.Plugins.AhpStatusChange.SendNotificationOnCreatePlugin: Create of Ahp Status Change",
      1,
      IsolationModeEnum.Sandbox,
      Id = "8E80150F-97F0-4C85-851C-E407828A6053")]
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
