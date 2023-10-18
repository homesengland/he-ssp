using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.Plugins.Handlers.LoanApplications;

namespace HE.CRM.Plugins.Plugins.LoanApplications
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_Loanapplication.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.LoanApplications.CreateDocumentLocationOnCreatePlugin: Create of Loan Application",
       1000,
       IsolationModeEnum.Sandbox,
       Id = "9de0cc67-e9e4-482e-8366-26b44fe6f2c1")]
    public class CreateDocumentLocationOnCreatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CreateDocumentLocationOnCreatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateDocumentLocationOnCreateHandler>());
        }
        #endregion
    }
}
