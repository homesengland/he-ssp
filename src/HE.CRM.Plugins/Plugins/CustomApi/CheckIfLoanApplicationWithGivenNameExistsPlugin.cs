using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_checkifloanapplicationwithgivennameexists",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.CheckIfLoanApplicationWithGivenNameExistsPlugin: invln_checkifloanapplicationwithgivennameexists",
    1,
    IsolationModeEnum.Sandbox,
    Id = "b1154182-cea2-4e51-8efb-2787e6c6d205")]
    public class CheckIfLoanApplicationWithGivenNameExistsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CheckIfLoanApplicationWithGivenNameExistsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfLoanApplicationWithGivenNameExistsHandler>());
        }
        #endregion
    }
}
