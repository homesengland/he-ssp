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

        public invln_Claim GetClaimForAllocationDeliveryPhase(Guid deliveryPhaseId, int milestone)
        {
            var claim = new invln_Claim();

            var query_invln_deliveryphase = deliveryPhaseId.ToString();
            var query_invln_Milestone = milestone;

            var query = new QueryExpression(invln_Claim.EntityLogicalName);
            query.ColumnSet.AddColumns(
                invln_Claim.Fields.invln_ClaimSubmissionDate,
                invln_Claim.Fields.invln_IncurredCosts,
                invln_Claim.Fields.invln_Milestone,
                invln_Claim.Fields.invln_MilestoneDate,
                invln_Claim.Fields.invln_AmountApportionedtoMilestone,
                invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone,
                invln_Claim.Fields.invln_RequirementsConfirmation,
                invln_Claim.Fields.StateCode,
                invln_Claim.Fields.StatusCode);
            query.Criteria.AddCondition(invln_Claim.Fields.invln_DeliveryPhase, ConditionOperator.Equal, query_invln_deliveryphase);
            query.Criteria.AddCondition(invln_Claim.Fields.invln_Milestone, ConditionOperator.Equal, query_invln_Milestone);

            claim = service.RetrieveMultiple(query).Entities.Select(x => x.ToEntity<invln_Claim>()).FirstOrDefault();
            return claim;
        }
    }
}

