using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.AHP.Plugins.Handlers.CustomApi;

namespace HE.CRM.AHP.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_checkifapplicationwithgivennameexists",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.CheckIfApplicationWithGivenNameExistsPlugin: invln_checkifapplicationwithgivennameexists",
    1,
    IsolationModeEnum.Sandbox,
    Id = "60ff7e31-8f99-4490-82e1-1766ad768f2a")]
    public class CheckIfApplicationWithGivenNameExistsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CheckIfApplicationWithGivenNameExistsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfApplicationWithGivenNameExistsHandler>());
        }
        #endregion
    }
}
