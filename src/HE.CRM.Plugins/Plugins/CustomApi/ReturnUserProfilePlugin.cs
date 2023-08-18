using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_returnuserprofile",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.ReturnUserProfile: invln_returnuserprofile",
    1,
    IsolationModeEnum.Sandbox,
    Id = "1be484ff-ab9b-4b0b-93c8-bebc330c4118")]
    public class ReturnUserProfilePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public ReturnUserProfilePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ReturnUserProfileHandler>());
        }
    }
}
