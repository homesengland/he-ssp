using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ISPs;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;


namespace HE.CRM.Plugins.Plugins.ISPs
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       invln_ISP.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "HE.CRM.Plugins.Plugins.Isps.CreateRecordsOnSentForApprovalChangePlugin: Create of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "24448fde-883d-42f3-89bb-4b88f6fe949d")]
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_ISP.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "invln_sendforapproval",
       "HE.CRM.Plugins.Plugins.Isps.CreateRecordsOnSentForApprovalChangePlugin: Update of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "7ed40632-bc2e-4013-a4ad-dfc52858417e",
       Image1Name = "PreImage",
       Image1Attributes = "invln_sendforapproval",
       Image1Type = ImageTypeEnum.PreImage)]
    public class CreateRecordsOnSentForApprovalChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public CreateRecordsOnSentForApprovalChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateRecordsOnSentForApprovalChangeHandler>());
        }
        #endregion
    }
}
