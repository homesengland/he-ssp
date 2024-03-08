using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_deactivatefrontdoorsite",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.DeactivateFrontDoorSitePlugin: invln_deactivatefrontdoorsite",
    1,
    IsolationModeEnum.Sandbox,
    Id = "59372077-44A9-478C-A1DB-79DE1964AF3E")]


    public class DeactivateFrontDoorSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public DeactivateFrontDoorSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeactivateFrontDoorSiteHandler>());
        }
        #endregion
    }
}
