using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_createahpproject",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.SetAhpProjectPlugin: invln_createahpproject",
    1,
    IsolationModeEnum.Sandbox,
    Id = "343D3A69-8AE0-4ABE-83CB-417684D0A7E7")]

    public class SetAhpProjectPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetAhpProjectHandler>());
        }
    }
}
