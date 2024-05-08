using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Consortium;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Consortium
{
    [CrmPluginRegistration(
    "invln_requestaddingmembertoconsortium",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.Consortium.RequestAddingMemberToConsortiumPlugin: invln_requestaddingmembertoconsortium",
    1,
    IsolationModeEnum.Sandbox,
    Id = "c62abe82-90f8-44f4-9907-5cddedc2523f")]
    public class RequestAddingMemberToConsortiumPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<RequestAddingMemberToConsortiumHandler>());
        }
    }
}
