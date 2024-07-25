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
      MessageNameEnum.Update,
      invln_reviewapproval.EntityLogicalName,
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      invln_reviewapproval.Fields.invln_status,
      "HE.CRM.Plugins.Plugins.ReviewsApprovals.SendNotificationOnCreatePlugin: Update of ReviewApproval",
      1,
      IsolationModeEnum.Sandbox,
      Image1Name = "PostImage",
      Image1Attributes = "invln_ispid,invln_status," + invln_reviewapproval.Fields.invln_ispid,
      Image1Type = ImageTypeEnum.PostImage,
      Image2Name = "PreImage",
      Image2Attributes = "invln_ispid,invln_status," + invln_reviewapproval.Fields.invln_ispid,
      Image2Type = ImageTypeEnum.PostImage,
      Id = "CACFD0E5-91E5-4F00-8274-E14C2891DB99")]
    public class PostUpdate : PluginBase<DataverseContext>, IPlugin
    {
        #region Constructors

        public PostUpdate(string unsecureConfig, string secureConfig) : base(unsecureConfig, secureConfig)
        {
        }

        #endregion Constructors

        #region Base Methods Overrides

        public override void RegisterHandlers(CrmHandlerFactory<DataverseContext> handlerFactory, IList<ICrmHandler> registeredHandlers)
        {
            registeredHandlers.Add(handlerFactory.GetHandler<IspStatusOnReviewApprovalStatusChangeHandler>());
            registeredHandlers.Add(handlerFactory.GetHandler<CreateReviewApprovalForRiskTeamHandler>());
        }

        #endregion Base Methods Overrides
    }
}
