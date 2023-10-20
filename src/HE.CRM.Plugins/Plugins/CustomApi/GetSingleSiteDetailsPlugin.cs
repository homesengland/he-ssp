using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getsinglesitedetails",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetSingleSiteDetailsPlugin: invln_getsinglesitedetails",
    1,
    IsolationModeEnum.Sandbox,
    Id = "0b691aab-6947-4f17-a745-32a30766fd49")]
    public class GetSingleSiteDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetSingleSiteDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetSingleSiteDetailsHandler>());
        }
        #endregion
    }
}
