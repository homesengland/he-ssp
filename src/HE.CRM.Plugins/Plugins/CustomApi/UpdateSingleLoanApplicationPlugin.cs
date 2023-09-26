using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.Base.Plugins;
using HE.CRM.Plugins.Handlers.CustomApi;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.CustomApi
{
    [CrmPluginRegistration(
     "invln_updatesingleloanapplication",
     "none",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "",
     "HE.CRM.Plugins.Plugins.CustomApi.UpdateSingleLoanApplicationPlugin: invln_updatesingleloanapplication",
     1,
     IsolationModeEnum.Sandbox,
     Id = "44de6397-de42-4175-9dac-0d4551ae78a7")]
    public class UpdateSingleLoanApplicationPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public UpdateSingleLoanApplicationPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<UpdateSingleLoanApplicationHandler>());
        }
        #endregion
    }
}
