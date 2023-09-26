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
       "ownerid",
       "HE.CRM.Plugins.Plugins.LoanApplications.CheckIfOwnerCanBeChanged: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "02172d5d-f4aa-4865-b75e-c8dd127077b0",
       Image1Name = "PreImage", Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class CheckIfOwnerCanBeChangedPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CheckIfOwnerCanBeChangedPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CheckIfOwnerCanBeChangedHandler>());
        }
        #endregion
    }
}
