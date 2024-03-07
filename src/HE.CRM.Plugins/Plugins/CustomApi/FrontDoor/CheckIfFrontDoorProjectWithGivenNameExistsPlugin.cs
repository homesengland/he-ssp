using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi.FrontDoor;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi.FrontDoor
{
    [CrmPluginRegistration(
    "invln_checkiffrontdoorprojectwithgivennameexists",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.FrontDoor.CheckIfFrontDoorProjectWithGivenNameExistsPlugin: invln_checkiffrontdoorprojectwithgivennameexists",
    1,
    IsolationModeEnum.Sandbox,
    Id = "553277EE-7423-4F3F-AF5C-07514EDE863E")]

    public class CheckIfFrontDoorProjectWithGivenNameExistsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CheckIfFrontDoorProjectWithGivenNameExistsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfFrontDoorProjectWithGivenNameExistsHandler>());
        }
        #endregion
    }
}
