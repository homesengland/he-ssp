using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
        "invln_requesttoremovemember",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Consortium.invln_requesttoremovemember: invln_requesttoremovemember",
        1,
        IsolationModeEnum.Sandbox,
        Id = "b5af248f-07cc-474b-852c-1bf52f201688")]
    public class RemoveMemberFromConsortiumPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<RemoveMemberFromConsortiumHandler>());
        }
    }
}
