using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_getmultiplefrontdoorprojects",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.GetMultipleFrontDoorProjectsForAccountPlugin: invln_getmultiplefrontdoorprojects",
    1,
    IsolationModeEnum.Sandbox,
    Id = "07F3F176-DAC3-4C94-BAA0-D3999CF44FDE")]

    public class GetMultipleFrontDoorProjectsForAccountPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleFrontDoorProjectsForAccountPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleFrontDoorProjectsForAccountHandler>());
        }
        #endregion
    }
}

