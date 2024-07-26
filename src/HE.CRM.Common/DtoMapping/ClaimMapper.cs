using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Linq.Expressions;
using DataverseModel;
using HE.Base.Common.Extensions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class ClaimMapper
    {
        public static MilestoneClaimDto MapToMilestoneClaimDto(Entity recordData, int milestone, Guid claimId)
        {
            var result = new MilestoneClaimDto();

            if (milestone == (int)invln_Milestone.Acquisition &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_acquisitiondate) == null &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_acquisitionmilestoneclaimdate) == null)
            {
                return null;
            }
            if (milestone == (int)invln_Milestone.SoS &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_startonsitedate) == null &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_startonsitemilestoneclaimdate) == null)
            {
                return null;
            }
            if (milestone == (int)invln_Milestone.PC &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_completiondate) == null &&
                recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_completionmilestoneclaimdate) == null)
            {
                return null;
            }

            if (claimId == Guid.Empty)
            {
                result.Type = milestone;

                if (milestone == (int)invln_Milestone.Acquisition)
                {
                    result.AmountOfGrantApportioned = recordData.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_AcquisitionValue).Value;
                    result.PercentageOfGrantApportioned = recordData.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue).Value;
                }
                if (milestone == (int)invln_Milestone.SoS)
                {
                    result.AmountOfGrantApportioned = recordData.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_StartOnSiteValue).Value;
                    result.PercentageOfGrantApportioned = recordData.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue).Value;
                }
                if (milestone == (int)invln_Milestone.PC)
                {
                    result.AmountOfGrantApportioned = recordData.GetAliasedAttributeValue<Money>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_CompletionValue).Value;
                    result.PercentageOfGrantApportioned = recordData.GetAliasedAttributeValue<decimal?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_CompletionPercentageValue).Value;
                }
            }
            else
            {
                var aliasName = "";
                switch (milestone)
                {
                    case (int)invln_Milestone.Acquisition:
                        aliasName = "ClaimAcquisition";
                        break;
                    case (int)invln_Milestone.SoS:
                        aliasName = "ClaimSoS";
                        break;
                    case (int)invln_Milestone.PC:
                        aliasName = "ClaimPC";
                        break;
                }

                result.Type = recordData.GetAliasedAttributeValue<OptionSetValue>(aliasName, invln_Claim.Fields.invln_Milestone).Value;
                result.Status = recordData.GetAliasedAttributeValue<OptionSetValue>(aliasName, invln_Claim.Fields.invln_ExternalStatus).Value;
                result.AmountOfGrantApportioned = recordData.GetAliasedAttributeValue<Money>(aliasName, invln_Claim.Fields.invln_AmountApportionedtoMilestone).Value;
                result.PercentageOfGrantApportioned = recordData.GetAliasedAttributeValue<double?>(aliasName, invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone).HasValue ? (decimal)recordData.GetAliasedAttributeValue<double?>(aliasName, invln_Claim.Fields.invln_PercentageofGrantApportionedtoThisMilestone).Value : 0;
                result.AchievementDate = recordData.GetAliasedAttributeValue<DateTime?>(aliasName, invln_Claim.Fields.invln_MilestoneDate) ?? null;
                result.SubmissionDate = recordData.GetAliasedAttributeValue<DateTime?>(aliasName, invln_Claim.Fields.invln_ClaimSubmissionDate) ?? null;
                result.CostIncurred = recordData.GetAliasedAttributeValue<bool?>(aliasName, invln_Claim.Fields.invln_IncurredCosts);
                result.IsConfirmed = recordData.GetAliasedAttributeValue<bool?>(aliasName, invln_Claim.Fields.invln_RequirementsConfirmation);
            }

            if (milestone == (int)invln_Milestone.Acquisition)
            {
                result.ForecastClaimDate = recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_acquisitionmilestoneclaimdate).Value;
            }
            if (milestone == (int)invln_Milestone.SoS)
            {
                result.ForecastClaimDate = recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_startonsitemilestoneclaimdate).Value;
            }
            if (milestone == (int)invln_Milestone.PC)
            {
                result.ForecastClaimDate = recordData.GetAliasedAttributeValue<DateTime?>("DeliveryPhase", invln_DeliveryPhase.Fields.invln_completionmilestoneclaimdate).Value;
            }

            return result;
        }
    }
}
