using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_getfilelocationforapplicationloan",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.GetFileLocationForApplicationLoanPlugin: invln_getfilelocationforapplicationloan",
    1,
    IsolationModeEnum.Sandbox,
    Id = "cd437ba4-223f-43e4-b07a-f2e9f2dd1a01")]
    public class GetFileLocationForApplicationLoanPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public GetFileLocationForApplicationLoanPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<GetFileLocationForApplicationLoanHandler>());
        }
        #endregion
    }
}
