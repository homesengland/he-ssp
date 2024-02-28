using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.AHP.Plugins.Handlers.CustomApi.Site;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi.Site
{
    [CrmPluginRegistration(
        "invln_checkifsitewithgivennameexists",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.Site.CheckIfSiteWithGivenNameExistsPlugin: invln_checkifsitewithgivennameexists",
        1,
        IsolationModeEnum.Sandbox,
        Id = "933749E6-6642-4C64-A2B7-9B4592654EBE")]

    public class CheckIfSiteWithGivenNameExistsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public CheckIfSiteWithGivenNameExistsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfSiteWithGivenNameExistsHandler>());
        }
    }
}
