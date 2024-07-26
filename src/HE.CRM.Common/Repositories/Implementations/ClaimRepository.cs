using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataverseModel;
using HE.Base.Repositories;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.Common.Repositories.Implementations
{
    public class ClaimRepository : CrmEntityRepository<invln_Claim, DataverseContext>, IClaimRepository
    {
        public ClaimRepository(CrmRepositoryArgs args) : base(args)
        {
        }

        public IEnumerable<invln_Claim> GetClaimsForAllocation(Guid allocationId, bool onlyApproved, params string[] claimColumns)
        {
            var query = new QueryExpression(invln_Claim.EntityLogicalName);
            query.ColumnSet.AddColumns(claimColumns);
            query.Criteria.AddCondition(invln_Claim.Fields.invln_Allocation, ConditionOperator.Equal, allocationId);

            if (onlyApproved)
            {
                query.Criteria.AddCondition(invln_Claim.Fields.StatusCode, ConditionOperator.Equal, (int)invln_Claim_StatusCode.Approve);
            }

            return service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Claim>());
        }
    }
}

