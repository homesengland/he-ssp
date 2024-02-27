using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Site
{
    [CrmPluginRegistration(
        "invln_getsinglesite",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Site.GetSingleSitePlugin: invln_getsinglesite",
        1,
        IsolationModeEnum.Sandbox,
        Id = "66ACEBDC-1F13-40FA-89B4-94A2C3159ECF")]

    public class GetSingleSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetSingleSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleSiteHandler>());
        }
    }
}
