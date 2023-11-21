using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Handlers.LoanStatusChange;
using HE.CRM.Plugins.Handlers.ReviewsApprovals;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Plugins.ReviewsApprovals
{

    [CrmPluginRegistration(
      MessageNameEnum.Create,
      invln_reviewapproval.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.ReviewsApprovals.SendNotificationOnCreatePlugin: Create of ReviewApproval",
      1,
      IsolationModeEnum.Sandbox,
      Image1Name = "PostImage",
      Image1Attributes = "invln_ispid,invln_status,",
      Image1Type = ImageTypeEnum.PostImage,
      Id = "7F9034CC-71AD-46B3-AD31-93E10FF7643F")]

    [CrmPluginRegistration(
      MessageNameEnum.Update,
      invln_reviewapproval.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "",
      "HE.CRM.Plugins.Plugins.ReviewsApprovals.SendNotificationOnCreatePlugin: Update of ReviewApproval",
      1,
      IsolationModeEnum.Sandbox,
      Image1Name = "PostImage",
      Image1Attributes = "invln_ispid,invln_status,",
      Image1Type = ImageTypeEnum.PostImage,
      Id = "CACFD0E5-91E5-4F00-8274-E14C2891DB99")]

    public class IspStatusOnReviewApprovalStatusChangePlugin : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors
        public IspStatusOnReviewApprovalStatusChangePlugin(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }
        #endregion

        #region Base Methods Overrides
        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<IspStatusOnReviewApprovalStatusChangeHandler>());
        }
        #endregion
    }
}
