using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_getahpproject_v2",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetAhpProjectPlugin: invln_getahpproject_v2",
    1,
    IsolationModeEnum.Sandbox,
    Id = "C6AC9ED1-4D3A-43DD-BF99-31D68ABE0FF3")]

    public class GetAhpProjectV2Plugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAhpProjectV2Handler>());
        }
    }
}
