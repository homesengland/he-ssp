using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_getmultiplefdprojects",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetMultipleFrontDoorProjectsPlugin: invln_getmultiplefdprojects",
    1,
    IsolationModeEnum.Sandbox,
    Id = "89112FA2-82CA-4EFE-B86C-0D20473557EA")]

    public class GetMultipleFrontDoorProjectsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleFrontDoorProjectsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleFrontDoorProjectsHandler>());
        }
        #endregion
    }
}
