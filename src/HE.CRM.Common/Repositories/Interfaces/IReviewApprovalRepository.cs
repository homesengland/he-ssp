using System;
using System.Collections.Generic;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Repositories.Interfaces
{
    public interface IReviewApprovalRepository : ICrmEntityRepository<invln_reviewapproval, DataverseContext>
    {
        List<invln_reviewapproval> GetReviewApprovalsForIsp(EntityReference invln_isp, invln_reviewerapproverset? approvalType);
    }
}
