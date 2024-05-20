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
    "invln_getahpprojects",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetAhpProjectsPlugin: invln_getahpprojects",
    1,
    IsolationModeEnum.Sandbox,
    Id = "EBF846F0-4765-478D-99CA-64632863EDB1")]
    public class GetAhpProjectsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetAhpProjectsHandler>());
        }
    }
}
