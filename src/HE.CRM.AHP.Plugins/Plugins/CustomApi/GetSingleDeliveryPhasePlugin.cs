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
   "invln_getsingledeliveryphase",
   "none",
   StageEnum.PostOperation,
   ExecutionModeEnum.Synchronous,
   "",
   "HE.CRM.Plugins.Plugins.CustomApi.GetSingleDeliveryPhasePlugin: invln_getsingledeliveryphase",
   1,
   IsolationModeEnum.Sandbox,
   Id = "90DC41BD-BBD8-435F-AF5C-3EF889E0A6D7")]
    public class GetSingleDeliveryPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetSingleDeliveryPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleDeliveryPhaseHandler>());
        }
        #endregion
    }
}
