using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Allocation
{
    [CrmPluginRegistration(
    "invln_getallocationclaims",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Allocation.GetAllocationClaimsPlugin: invln_getallocationclaims",
    1,
    IsolationModeEnum.Sandbox,
    Id = "17FA7365-12A8-419D-8928-B7489B54CF19")]
    public class GetAllocationClaimsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetAllocationClaimsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAllocationClaimsHandler>());
        }


    }
}
