using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Plugins.Handlers;
using HE.CRM.Plugins.Services.LoanStatusChange;
using HE.CRM.Plugins.Services.ReviewsApprovals;

namespace HE.CRM.Plugins.Handlers.ReviewsApprovals
{
    public class IspStatusOnReviewApprovalStatusChangeHandler : CrmEntityHandlerBase<invln_reviewapproval, DataverseContext>
    {
        #region Fields

        private invln_reviewapproval Target => ExecutionData.Target;
        private invln_reviewapproval PostImage => ExecutionData.PostImage;

        #endregion
        #region Base Methods Overrides
        public override bool CanWork()
        {
            return Target != null;
        }

        public override void DoWork()
        {
            CrmServicesFactory.Get<IReviewsApprovalsService>().UpdateIspRelatedToApprovalsService(Target, PostImage);
        }

        #endregion
    }
}
