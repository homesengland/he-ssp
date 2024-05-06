using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Site
{
    [CrmPluginRegistration(
        "invln_checkifsitewithstrategicsitenameexists",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Site.CheckIfSiteWithStrategicSiteNameExistsPlugin: invln_checkifsitewithstrategicsitenameexists",
        1,
        IsolationModeEnum.Sandbox,
        Id = "E56583E4-5701-4F0F-8335-64F096B417B8")]


    public class CheckIfSiteWithStrategicSiteNameExistsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public CheckIfSiteWithStrategicSiteNameExistsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfSiteWithStrategicSiteNameExistsHandler>());
        }
    }
}
