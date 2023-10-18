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
       "HE.CRM.Plugins.Plugins.LoanApplications.AssignLoanToTmTeamOnCreate: Create of Loan Application",
       1000,
       IsolationModeEnum.Sandbox,
       Id = "BB64EDD5-BC2A-4E90-BD69-AC207F0D7473")]
    public class AssignLoanToTmTeamOnCreatePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public AssignLoanToTmTeamOnCreatePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<AssignLoanToTmTeamOnCreateHandler>());
        }
        #endregion
    }
}
