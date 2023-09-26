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
       "statuscode,invln_externalstatus",
       "HE.CRM.Plugins.Plugins.LoanApplications.SetFieldsWhenChangingStatusFromDraftPlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "c29a5e63-d5de-4883-8bdf-a37c12be214c",
       Image1Name = "PreImage", Image1Attributes = "statuscode,invln_externalstatus",
       Image1Type = ImageTypeEnum.PreImage)]
    public class SetFieldsWhenChangingStatusFromDraftPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SetFieldsWhenChangingStatusFromDraftPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetFieldsWhenChangingStatusFromDraftHandler>());
        }
        #endregion
    }
}
