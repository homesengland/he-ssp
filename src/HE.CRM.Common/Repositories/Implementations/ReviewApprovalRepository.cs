using System.Collections.Generic;
using System.Linq;
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
            var query = new QueryExpression(invln_reviewapproval.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    invln_reviewapproval.Fields.invln_reviewerapprover,
                    invln_reviewapproval.Fields.invln_status,
                    invln_reviewapproval.Fields.CreatedOn,
                    invln_reviewapproval.Fields.OwnerId
                )
            };

            query.Criteria.AddCondition(invln_reviewapproval.Fields.invln_ispid, ConditionOperator.Equal, invln_isp.Id);
            if (approvalType != null)
            {
                query.Criteria.AddCondition(invln_reviewapproval.Fields.invln_reviewerapprover, ConditionOperator.Equal, (int)approvalType);
            }

            var result = this.service.RetrieveMultiple(query);
            if (result != null && result.Entities.Count > 0)
            {
                reviewapprovals.AddRange(result.Entities.Select(x => x.ToEntity<invln_reviewapproval>()));
            }

            return reviewapprovals;
        }
    }
}
