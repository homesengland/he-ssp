using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
     "invln_updateuserprofile",
     "none",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.Plugins.Plugins.CustomApi.UpdateUserProfilePlugin: invln_updateuserprofile",
     1,
     IsolationModeEnum.Sandbox,
     Id = "004af2a6-5a78-4523-b529-5a0e4e5ce3ee")]
    public class UpdateUserProfilePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public UpdateUserProfilePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateUserProfileHandler>());
        }
    }
}
