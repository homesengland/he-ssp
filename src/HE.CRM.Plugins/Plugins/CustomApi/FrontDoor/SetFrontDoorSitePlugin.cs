using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_setfrontdoorsite",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.SetFrontDoorSitePlugin: invln_setfrontdoorsite",
    1,
    IsolationModeEnum.Sandbox,
    Id = "F4AB6765-F6C3-4BAE-B0B2-249E29DA2CAF")]


    public class SetFrontDoorSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetFrontDoorSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetFrontDoorSiteHandler>());
        }
        #endregion
    }
}
