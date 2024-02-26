using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Site
{
    [CrmPluginRegistration(
        "invln_getmultiplesites",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.GetMultipleSitesPlugin: invln_getmultiplesites",
        1,
        IsolationModeEnum.Sandbox,
        Id = "74384A99-7B2D-4F40-95B6-2FBF21A4F87D")]

    public class GetMultipleSitesPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public GetMultipleSitesPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleSitesHandler>());
        }
    }
}
