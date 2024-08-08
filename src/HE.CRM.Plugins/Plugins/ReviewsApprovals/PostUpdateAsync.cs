using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
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
        StageEnum.PostOperation,
        ExecutionModeEnum.Asynchronous,
        invln_reviewapproval.Fields.invln_status,
        "HE.CRM.Plugins.Plugins.ReviewsApprovals.PostUpdateAsync: Update of invln_reviewapproval",
        1,
        IsolationModeEnum.Sandbox,
        Id = "59755766-E93D-4A2A-9F1D-D7D4737F801F",
        Image1Name = "PreImage",
        Image1Attributes = "invln_status",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Name = "PostImage",
        Image2Attributes = "invln_status, invln_reviewapprovalid, invln_ispid",
        Image2Type = ImageTypeEnum.PostImage,
        DeleteAsyncOperation = true
    )]

    public class PostUpdateAsync : PluginBase<DataverseContext>, IPlugin
    {
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<NotificationAfterChangingReviewsApprovalStatusHandler>());
        }
    }
}



