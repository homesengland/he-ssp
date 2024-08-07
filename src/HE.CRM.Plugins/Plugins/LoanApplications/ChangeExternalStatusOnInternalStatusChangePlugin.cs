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
       "HE.CRM.Plugins.Plugins.LoanApplications.ChangeExternalStatusOnInternalStatusChangePlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "0c45565a-bd91-4610-ac44-fc10205e2aa1",
       Image1Name = "PreImage", Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class ChangeExternalStatusOnInternalStatusChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors

        public ChangeExternalStatusOnInternalStatusChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        #endregion Constructors

        #region Base Methods Overrides

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<ChangeExternalStatusOnInternalStatusChangeHandler>());
        }

        #endregion Base Methods Overrides
    }
}
