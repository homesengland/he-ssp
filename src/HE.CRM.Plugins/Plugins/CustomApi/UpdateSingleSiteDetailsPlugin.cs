using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
     "invln_updatesinglesitedetails",
     "none",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.Plugins.Plugins.CustomApi.UpdateSingleSiteDetailsPlugin: invln_updatesinglesitedetails",
     1,
     IsolationModeEnum.Sandbox,
     Id = "6cb2968f-905e-4d87-a375-7793bb6eccb2")]
    public class UpdateSingleSiteDetailsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public UpdateSingleSiteDetailsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateSingleSiteDetailsHandler>());
        }
        #endregion
    }
}
