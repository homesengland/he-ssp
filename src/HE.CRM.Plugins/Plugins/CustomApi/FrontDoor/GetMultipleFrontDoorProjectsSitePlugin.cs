using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
        "invln_getmultiplefrontdoorprojectssite",
        "none",
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "",
        "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetMultipleFrontDoorProjectsSitePlugin: invln_getmultiplefrontdoorprojectssite",
        1,
        IsolationModeEnum.Sandbox,
        Id = "5193DCAE-956B-4F1F-8B5E-015BDF13DD39")]
    public class GetMultipleFrontDoorProjectsSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleFrontDoorProjectsSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleFrontDoorProjectsSiteHandler>());
        }
        #endregion
    }
}
