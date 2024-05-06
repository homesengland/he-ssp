using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
    "invln_getconsortium",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Consortium.GetConsortiumPlugin: invln_getconsortium",
    1,
    IsolationModeEnum.Sandbox,
    Id = "bbe90236-66ec-40cb-8f64-8a09bd5f3eec")]
    public class GetConsortiumPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetConsortiumPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetConsortiumHandler>());
        }

    }
}
