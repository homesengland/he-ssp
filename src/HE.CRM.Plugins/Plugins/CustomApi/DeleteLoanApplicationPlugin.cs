using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
    "invln_deleteloanapplication",
    "none",
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "",
    "HE.CRM.Plugins.Plugins.CustomApi.DeleteLoanApplicationPlugin: invln_deleteloanapplication",
    1,
    IsolationModeEnum.Sandbox,
    Id = "3887ace8-9550-46f1-9602-02b90b9faa19")]
    public class DeleteLoanApplicationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        public DeleteLoanApplicationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<DeleteLoanApplicationHandler>());
        }
    }
}
