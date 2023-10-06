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
       "HE.CRM.Plugins.Plugins.LoanApplications.SendEmailToNewOwnerPlugin: Update of Loan Application",
       1,
       IsolationModeEnum.Sandbox,
       Id = "151531d8-bde8-4661-aff9-1a5e759e525f",
       Image1Name = "PreImage", Image1Attributes = "",
       Image1Type = ImageTypeEnum.PreImage)]
    public class SendEmailToNewOwnerPlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public SendEmailToNewOwnerPlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SendEmailToNewOwnerHandler>());
        }
        #endregion
    }
}
