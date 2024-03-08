using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_deactivatefrontdoorproject",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.DeactivateFrontDoorProjectPlugin: invln_deactivatefrontdoorproject",
    1,
    IsolationModeEnum.Sandbox,
    Id = "05E06026-AD07-4CF9-A98C-62870FD08116")]


    public class DeactivateFrontDoorProjectPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public DeactivateFrontDoorProjectPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeactivateFrontDoorProjectHandler>());
        }
        #endregion
    }
}
