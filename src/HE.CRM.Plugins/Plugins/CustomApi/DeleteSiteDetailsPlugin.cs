using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_deletesitedetails",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.DeleteSiteDetailsPlugin: invln_deletesitedetails",
    1,
    IsolationModeEnum.Sandbox,
    Id = "144c73bc-e888-4eac-abca-0e6e2054aa82")]
    public class DeleteSiteDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public DeleteSiteDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeleteSiteDetailsHandler>());
        }
    }
}
