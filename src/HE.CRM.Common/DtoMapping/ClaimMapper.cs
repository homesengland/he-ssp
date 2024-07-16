using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class ClaimMapper
    {

        public static MilestoneClaimDto MapToMilestoneClaimDto(invln_Claim claim)
        {
            var result = new MilestoneClaimDto()
            {
                Type = claim.invln_Milestone.Value,
                Status = claim.StateCode.Value,
                AmountOfGrantApportioned = claim.invln_AmountApportionedtoMilestone.Value,
                PercentageOfGrantApportioned = (decimal) claim.invln_PercentageofGrantApportionedtoThisMilestone.Value,
                ForecastClaimDate = claim.invln_MilestoneDate.Value,
                ClaimDate = claim.invln_ClaimSubmissionDate.Value,
                CostIncurred = claim.invln_IncurredCosts,
                IsConfirmed = claim.invln_RequirementsConfirmation,
            };
            return result;
        }
    }
}
