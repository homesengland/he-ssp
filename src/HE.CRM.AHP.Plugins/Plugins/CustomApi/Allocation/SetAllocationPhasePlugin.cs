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
    "invln_setallocationphase",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Allocation.SetAllocationPhasePlugin: invln_setallocationphase",
    1,
    IsolationModeEnum.Sandbox,
    Id = "EA5F7362-6CCE-4150-8EBA-C02885DF7E3D")]


    public class SetAllocationPhasePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public SetAllocationPhasePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetAllocationPhaseHandler>());
        }

    }
}
