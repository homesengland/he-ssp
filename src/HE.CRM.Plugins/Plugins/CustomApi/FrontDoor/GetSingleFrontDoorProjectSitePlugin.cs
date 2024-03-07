using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_getsinglefrontdoorprojectsite",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetSingleFrontDoorProjectSitePlugin: invln_getsinglefrontdoorprojectsite",
    1,
    IsolationModeEnum.Sandbox,
    Id = "12740757-7338-44AD-A941-F9E326E6C07E")]

    public class GetSingleFrontDoorProjectSitePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetSingleFrontDoorProjectSitePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleFrontDoorProjectSiteHandler>());
        }
        #endregion
    }
}





