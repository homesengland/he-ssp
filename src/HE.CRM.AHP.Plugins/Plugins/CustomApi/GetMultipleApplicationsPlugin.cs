using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getmultipleahpapplications",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetMultipleApplicationsPlugin: invln_getmultipleahpapplications",
    1,
    IsolationModeEnum.Sandbox,
    Id = "fac74145-9d04-4fe6-b508-4ac139102059")]
    public class GetMultipleApplicationsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetMultipleApplicationsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetMultipleApplicationsHandler>());
        }
        #endregion
    }
}
