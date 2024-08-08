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
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      invln_reviewapproval.Fields.invln_status,
      "HE.CRM.Plugins.Plugins.ReviewsApprovals.PostUpdate: Update of invln_reviewapproval",
      1,
      IsolationModeEnum.Sandbox,
      Image1Name = "PostImage",
      Image1Attributes = "invln_ispid, invln_status",
      Image1Type = ImageTypeEnum.PostImage,
      Image2Name = "PreImage",
      Image2Attributes = "invln_ispid, invln_status",
      Image2Type = ImageTypeEnum.PreImage,
      Id = "CACFD0E5-91E5-4F00-8274-E14C2891DB99"
    )]
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
