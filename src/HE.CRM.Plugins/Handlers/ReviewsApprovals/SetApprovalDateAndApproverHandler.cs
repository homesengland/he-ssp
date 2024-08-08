using System;
using DataverseModel;
using HE.Base.Plugins.Attributes;
using HE.Base.Plugins.Common.Constants;
using HE.Base.Plugins.Handlers;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Handlers.ReviewsApprovals
{
    /// <summary>
    /// Set Review / Approval Date and Reviewed / Approved By on the invln_reviewapproval entity
    /// when invln_status field is changed to Reviewed or Approved.
    /// </summary>
    [CrmMessage(CrmMessage.Update)]
    [CrmProcessingStage(CrmProcessingStepStages.Preoperation)]
    public class SetApprovalDateAndApproverHandler : CrmEntityHandlerBase<invln_reviewapproval, DataverseContext>
    {
        public override bool CanWork()
        {
            if (ValueChanged(invln_reviewapproval.Fields.invln_status, invln_reviewerapproverset.HoFReview) ||
                ValueChanged(invln_reviewapproval.Fields.invln_status, invln_reviewerapproverset.RiskApproval))
            {
                return true;
            }

            return false;
        }

        public override void DoWork()
        {
            ExecutionData.Target.invln_reviewapprovaldate = DateTime.UtcNow;
            ExecutionData.Target.invln_reviewedapprovedbyid = new EntityReference(SystemUser.EntityLogicalName, ExecutionData.Context.UserId);
        }
    }
}
