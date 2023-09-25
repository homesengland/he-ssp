using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using HE.CRM.Plugins.Handlers.Contract;

namespace HE.CRM.Plugins.Plugins.LoanApplications
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_contract.EntityLogicalName,
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.Contract.RejectAddingWhenRelatedLoanInApprovedStatusPlugin: Create of Contract",
       1,
       IsolationModeEnum.Sandbox,
       Id = "0560bc02-227e-40d4-ac52-a5ab80d1331f")]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        invln_contract.EntityLogicalName,
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "invln_loanapplicationid",
        "HE.CRM.Plugins.Plugins.Contract.RejectAddingWhenRelatedLoanInApprovedStatusPlugin: Update of Contract",
        1,
        IsolationModeEnum.Sandbox,
        Id = "66d7ceea-e430-42c3-811d-462bf2352c87",
        Image1Name = "PreImage", Image1Attributes = "",
        Image1Type = ImageTypeEnum.PreImage)]
    public class RejectAddingWhenRelatedLoanInApprovedStatusPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructor
        public RejectAddingWhenRelatedLoanInApprovedStatusPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<RejectAddingWhenRelatedLoanInApprovedStatusHandler>());
        }
        #endregion
    }
}
