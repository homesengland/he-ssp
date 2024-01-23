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
    "invln_getmultipledeliveryphase",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetMultipleDeliveryPhasePlugin: invln_getmultipledeliveryphase",
    1,
    IsolationModeEnum.Sandbox,
    Id = "6BA18DEC-AA6C-4B0D-B711-4F118CA88C45")]
    public class GetMultipleDeliveryPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleDeliveryPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleDeliveryPhaseHandler>());
        }
        #endregion
    }
}
