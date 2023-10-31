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
       "statuscode",
       "HE.CRM.Plugins.Plugins.LoanApplications.CreateStandardConditionsPlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "ba16e1b4-2d97-47c4-9f1c-9a7243481317")]
    public class CreateStandardConditionsPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CreateStandardConditionsPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateStandardConditionsHandler>());
        }
        #endregion
    }
}
