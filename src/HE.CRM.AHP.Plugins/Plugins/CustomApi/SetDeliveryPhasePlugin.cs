using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
   "invln_setdeliveryphase",
   "none",
   StageEnum.PostOperation,
   ExecutionModeEnum.Synchronous,
   "",
   "HE.CRM.Plugins.Plugins.CustomApi.SetDeliveryPhasePlugin: invln_setdeliveryphase",
   1,
   IsolationModeEnum.Sandbox,
   Id = "E947EAB2-47F2-4141-9FCF-FC2EA5F87248")]
    public class SetDeliveryPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetDeliveryPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetDeliveryPhaseHandler>());
        }
        #endregion
    }
}
