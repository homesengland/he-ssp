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

        public static MilestoneClaimDto MapToMilestoneClaimDto(invln_DeliveryPhase deliveryphase, int milestone, invln_Claim claim = null)
        {
            var result = new MilestoneClaimDto();
            if (claim == null)
            {
                result.Type = milestone;

                if (milestone == (int)invln_Milestone.Acquisition)
                {
                    result.AmountOfGrantApportioned = deliveryphase.invln_AcquisitionValue.Value;
                    result.PercentageOfGrantApportioned = deliveryphase.invln_AcquisitionPercentageValue.Value;
                }
                if (milestone == (int)invln_Milestone.SoS)
                {
                    result.AmountOfGrantApportioned = deliveryphase.invln_StartOnSiteValue.Value;
                    result.PercentageOfGrantApportioned = deliveryphase.invln_StartOnSitePercentageValue.Value;
                }
                if (milestone == (int)invln_Milestone.PC)
                {
                    result.AmountOfGrantApportioned = deliveryphase.invln_CompletionValue.Value;
                    result.PercentageOfGrantApportioned = deliveryphase.invln_CompletionPercentageValue.Value;
                }
            }
            else
            {
                result.Type = claim.invln_Milestone.Value;
                result.Status = claim.StatusCode.Value;
                result.AmountOfGrantApportioned = claim.invln_AmountApportionedtoMilestone.Value;
                result.PercentageOfGrantApportioned = claim.invln_PercentageofGrantApportionedtoThisMilestone.HasValue ? (decimal)claim.invln_PercentageofGrantApportionedtoThisMilestone.Value : 0;
                result.AchivmentDate = claim.invln_MilestoneDate ?? null;
                result.SubmisstionDate = claim.invln_ClaimSubmissionDate ?? null;
                result.CostIncurred = claim.invln_IncurredCosts;
                result.IsConfirmed = claim.invln_RequirementsConfirmation;
            }

            if (milestone == (int)invln_Milestone.Acquisition)
            {
                result.ForecastClaimDate = deliveryphase.invln_acquisitionmilestoneclaimdate.Value;
            }
            if (milestone == (int)invln_Milestone.SoS)
            {
                result.ForecastClaimDate = deliveryphase.invln_startonsitemilestoneclaimdate.Value;
            }
            if (milestone == (int)invln_Milestone.PC)
            {
                result.ForecastClaimDate = deliveryphase.invln_completionmilestoneclaimdate.Value;
            }

            return result;
        }
    }
}
