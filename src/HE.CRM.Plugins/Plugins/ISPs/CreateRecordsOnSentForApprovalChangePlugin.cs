using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ISPs;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.CRM.Plugins.Plugins.ISPs
{
    [CrmPluginRegistration(
       MessageNameEnum.Update,
       invln_ISP.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "invln_sendforapproval",
       "HE.CRM.Plugins.Plugins.Isps.PostUpdate: Update of Isp",
       1,
       IsolationModeEnum.Sandbox,
       Id = "7ed40632-bc2e-4013-a4ad-dfc52858417e",
       Image1Name = "PreImage",
       Image1Attributes = invln_ISP.Fields.invln_SendforApproval + "," +
        invln_ISP.Fields.invln_Loanapplication + "," +
        invln_ISP.Fields.invln_ApprovalLevelNew,
       Image1Type = ImageTypeEnum.PreImage)]
    public class CreateRecordsOnSentForApprovalChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<CreateRecordsOnSentForApprovalChangeHandler>());
        }
    }
}
