using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_setahpapplication",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.SetApplicationPlugin: invln_setahpapplication",
    1,
    IsolationModeEnum.Sandbox,
    Id = "f72756a7-3800-426a-96c7-a0d16995a6d0")]
    public class SetApplicationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetApplicationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetApplicationHandler>());
        }
        #endregion
    }
}
