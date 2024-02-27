using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Site
{
    [CrmPluginRegistration(
        "invln_setsite",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Site.GetMultipleSitesPlugin: invln_setsite",
        1,
        IsolationModeEnum.Sandbox,
        Id = "7BFB9FD1-3BB7-4021-B90C-2115685ACD43")]

    public class SetSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public SetSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetSiteHandler>());
        }
    }
}
