using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Allocation
{
    [CrmPluginRegistration(
    "invln_getallocation",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Allocation.GetAllocationPlugin: invln_getallocation",
    1,
    IsolationModeEnum.Sandbox,
    Id = "231CC8F7-52E2-4C2A-8B2F-D8E9EB26F088")]
    public class GetAllocationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetAllocationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAllocationHandler>());
        }
    }
}
