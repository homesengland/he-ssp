using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ReviewApprovalRepository : CrmEntityRepository<invln_reviewapproval, DataverseContext>, IReviewApprovalRepository
    {
        public ReviewApprovalRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public List<invln_reviewapproval> GetReviewApprovalsForIsp(EntityReference invln_isp, invln_reviewerapproverset? approvalType)
        {
            var reviewapprovals = new List<invln_reviewapproval>();
            var qe = new QueryExpression(invln_reviewapproval.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    nameof(invln_reviewapproval.invln_reviewerapprover).ToLower(),
                    nameof(invln_reviewapproval.invln_status).ToLower())
            };

            qe.Criteria.AddCondition(nameof(invln_reviewapproval.invln_ispid).ToLower(), ConditionOperator.Equal, invln_isp.Id);
            if (approvalType != null)
            {
                qe.Criteria.AddCondition(nameof(invln_reviewapproval.invln_reviewerapprover).ToLower(), ConditionOperator.Equal, (int)approvalType);
            }

            var result = this.service.RetrieveMultiple(qe);
            if (result != null && result.Entities.Count > 0)
            {
                reviewapprovals.AddRange(result.Entities.Select(x => x.ToEntity<invln_reviewapproval>()));
            }

            return reviewapprovals;
        }
    }
}
