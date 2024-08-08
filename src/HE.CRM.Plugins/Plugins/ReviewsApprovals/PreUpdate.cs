using System.Collections.Generic;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.ReviewsApprovals;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.ReviewsApprovals
{
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_reviewapproval.EntityLogicalName,
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      invln_reviewapproval.Fields.invln_status,
      "HE.CRM.Plugins.Plugins.ReviewsApprovals.PreUpdate: Update of invln_reviewapproval",
      1,
      IsolationModeEnum.Sandbox,
      Image1Name = "PreImage",
      Image1Attributes = "invln_status",
      Image1Type = ImageTypeEnum.PreImage,
      Id = "D99F0F98-9236-4E5C-94A2-997F10B42B96"
    )]
    public class PreUpdate : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<SetApprovalDateAndApproverHandler>());
        }
    }
}
