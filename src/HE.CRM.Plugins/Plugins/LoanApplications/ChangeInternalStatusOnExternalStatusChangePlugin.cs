using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.Plugins.Handlers.LoanApplications;

namespace HE.CRM.Plugins.Plugins.LoanApplications
{
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_Loanapplication.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "invln_externalstatus",
       "HE.CRM.Plugins.Plugins.LoanApplications.ChangeInternalStatusOnExternalStatusChangePlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "b5774d7d-8838-4856-b289-62c52d61fb33",
       Image1Name = "PreImage", Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class ChangeInternalStatusOnExternalStatusChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public ChangeInternalStatusOnExternalStatusChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeInternalStatusOnExternalStatusChangeHandler>());
        }
    }
}
