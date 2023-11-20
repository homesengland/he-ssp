using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IReviewApprovalRepository : ICrmEntityRepository<invln_reviewapproval, DataverseContext>
    {
        List<invln_reviewapproval> GetReviewApprovalsForIsp(invln_reviewerapproverset approvalType);
    }
}
