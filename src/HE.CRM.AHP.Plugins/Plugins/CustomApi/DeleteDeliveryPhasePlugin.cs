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
    "invln_deletedeliveryphase",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.DeleteDeliveryPhasePlugin: invln_deletehometype",
    1,
    IsolationModeEnum.Sandbox,
    Id = "BFABEE16-182C-4BB0-B90B-4F232352D762")]
    public class DeleteDeliveryPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public DeleteDeliveryPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeleteDeliveryPhaseHandler>());
        }
        #endregion
    }
}
