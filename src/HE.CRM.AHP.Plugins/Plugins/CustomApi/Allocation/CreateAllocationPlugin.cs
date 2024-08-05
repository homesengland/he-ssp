using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Allocation;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Allocation
{
    [CrmPluginRegistration(
        "invln_createahpallocation",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.AHP.Plugins.Plugins.CustomApi.Allocation.CreateAllocationPlugin: invln_createahpallocation",
        1,
        IsolationModeEnum.Sandbox,
        Id = "037EE38E-71DE-4FFC-A6AD-169955EFBD18"
    )]
    public class CreateAllocationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public CreateAllocationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateAllocationHandler>());
        }

    }
}
