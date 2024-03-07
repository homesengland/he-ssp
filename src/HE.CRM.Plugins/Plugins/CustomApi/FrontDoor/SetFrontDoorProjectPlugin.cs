using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_setfrontdoorproject",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.SetFrontDoorProjectPlugin: invln_setfrontdoorproject",
    1,
    IsolationModeEnum.Sandbox,
    Id = "077DCCE6-2266-4423-BF00-77E1149C26D4")]


    public class SetFrontDoorProjectPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetFrontDoorProjectPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetFrontDoorProjectHandler>());
        }
        #endregion
    }
}
