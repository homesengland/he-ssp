using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_deletehometype",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.DeleteHomeTypePlugin: invln_deletehometype",
    1,
    IsolationModeEnum.Sandbox,
    Id = "24bfdbe3-6662-4d68-bbc2-3cb3c00d33a4")]
    public class DeleteHomeTypePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public DeleteHomeTypePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeleteHomeTypeHandler>());
        }
        #endregion
    }
}
