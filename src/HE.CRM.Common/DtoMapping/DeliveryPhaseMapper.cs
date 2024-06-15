using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class DeliveryPhaseMapper
    {
        public static DeliveryPhaseDto MapRegularEntityToDto(invln_DeliveryPhase deliveryPhase, List<invln_homesindeliveryphase> homesInDeliveryPhase)
        {
            var deliveryPhaseDto = new DeliveryPhaseDto()
            {
                name = deliveryPhase.invln_phasename,
                createdOn = deliveryPhase.CreatedOn,
                newBuildActivityType = deliveryPhase.invln_buildactivitytype?.Value,
                rehabBuildActivityType = deliveryPhase.invln_rehabactivitytype?.Value,
                isReconfigurationOfExistingProperties = deliveryPhase.invln_reconfiguringexistingproperties,
                numberOfHomes = new Dictionary<string, int?>(),
                acquisitionDate = deliveryPhase.invln_acquisitiondate,
                acquisitionPaymentDate = deliveryPhase.invln_acquisitionmilestoneclaimdate,
                startOnSiteDate = deliveryPhase.invln_startonsitedate,
                startOnSitePaymentDate = deliveryPhase.invln_startonsitemilestoneclaimdate,
                completionDate = deliveryPhase.invln_completiondate,
                completionPaymentDate = deliveryPhase.invln_completionmilestoneclaimdate,
                typeOfHomes = MapTypeOfHome(deliveryPhase.invln_nbrh),
                requiresAdditionalPayments = MapYesNo(deliveryPhase.invln_urbrequestingearlymilestonepayments),
                isCompleted = deliveryPhase.invln_iscompleted,
                acquisitionValue = deliveryPhase.invln_AcquisitionValue?.Value,
                acquisitionPercentageValue = deliveryPhase.invln_AcquisitionPercentageValue.HasValue ? deliveryPhase.invln_AcquisitionPercentageValue.Value / 100 : deliveryPhase.invln_AcquisitionPercentageValue,
                startOnSiteValue = deliveryPhase.invln_StartOnSiteValue?.Value,
                startOnSitePercentageValue = deliveryPhase.invln_StartOnSitePercentageValue.HasValue ? deliveryPhase.invln_StartOnSitePercentageValue / 100 : deliveryPhase.invln_StartOnSitePercentageValue,
                completionValue = deliveryPhase.invln_CompletionValue?.Value,
                completionPercentageValue = deliveryPhase.invln_CompletionPercentageValue.HasValue ? deliveryPhase.invln_CompletionPercentageValue / 100 : deliveryPhase.invln_CompletionPercentageValue,
                claimingtheMilestoneConfirmed = deliveryPhase.invln_ClaimingtheMilestoneConfirmed,
                sumOfCalculatedFounds = deliveryPhase.invln_sumofcalculatedfounds?.Value,
                statusCode = deliveryPhase.StatusCode.Value
            };

            if (deliveryPhase.Id != null)
            {
                deliveryPhaseDto.id = deliveryPhase.Id.ToString();
            }

            if (deliveryPhase.invln_Application?.Id != null)
            {
                deliveryPhaseDto.applicationId = deliveryPhase.invln_Application.Id.ToString();
            }

            if (homesInDeliveryPhase != null)
            {
                foreach (var home in homesInDeliveryPhase.Where(x => x != null && x.invln_hometypelookup != null))
                {
                    deliveryPhaseDto.numberOfHomes.Add(home.invln_hometypelookup.Id.ToString(), home?.invln_numberofhomes);
                }
            }

            return deliveryPhaseDto;
        }

        public static invln_DeliveryPhase MapDtoToRegularEntity(DeliveryPhaseDto deliveryPhaseDto, string applicationId = null)
        {
            var deliveryPhase = new invln_DeliveryPhase()
            {
                invln_phasename = deliveryPhaseDto.name,
                invln_buildactivitytype = MapNullableIntToOptionSetValue(deliveryPhaseDto.newBuildActivityType),
                invln_rehabactivitytype = MapNullableIntToOptionSetValue(deliveryPhaseDto.rehabBuildActivityType),
                invln_reconfiguringexistingproperties = deliveryPhaseDto.isReconfigurationOfExistingProperties,
                invln_acquisitiondate = deliveryPhaseDto.acquisitionDate,
                invln_acquisitionmilestoneclaimdate = deliveryPhaseDto.acquisitionPaymentDate,
                invln_startonsitedate = deliveryPhaseDto.startOnSiteDate,
                invln_startonsitemilestoneclaimdate = deliveryPhaseDto.startOnSitePaymentDate,
                invln_completiondate = deliveryPhaseDto.completionDate,
                invln_completionmilestoneclaimdate = deliveryPhaseDto.completionPaymentDate,
                invln_nbrh = MapTypeOfHome(deliveryPhaseDto.typeOfHomes),
                invln_urbrequestingearlymilestonepayments = MapYesNo(deliveryPhaseDto.requiresAdditionalPayments),
                invln_invln_homesindeliveryphase_deliveryphasel = MapHomesInDeliveryPhase(deliveryPhaseDto),
                invln_iscompleted = deliveryPhaseDto.isCompleted,
                invln_AcquisitionPercentageValue = deliveryPhaseDto.acquisitionPercentageValue.HasValue ? deliveryPhaseDto.acquisitionPercentageValue * 100 : deliveryPhaseDto.acquisitionPercentageValue,
                invln_StartOnSitePercentageValue = deliveryPhaseDto.startOnSitePercentageValue.HasValue ? deliveryPhaseDto.startOnSitePercentageValue * 100 : deliveryPhaseDto.startOnSitePercentageValue,
                invln_CompletionPercentageValue = deliveryPhaseDto.completionPercentageValue.HasValue ? deliveryPhaseDto.completionPercentageValue * 100 : deliveryPhaseDto.completionPercentageValue,
                invln_ClaimingtheMilestoneConfirmed = deliveryPhaseDto.claimingtheMilestoneConfirmed,
            };

            if (deliveryPhaseDto.numberOfHomes.Count > 0)
            {
                deliveryPhase.invln_NoofHomes = deliveryPhaseDto.numberOfHomes.Sum(x => x.Value);
            }

            if (deliveryPhaseDto.id != null)
            {
                deliveryPhase.Id = new Guid(deliveryPhaseDto.id);
            }

            if (Guid.TryParse(applicationId ?? deliveryPhaseDto.applicationId, out var applicationGuid))
            {
                deliveryPhase.invln_Application = new EntityReference(invln_scheme.EntityLogicalName, applicationGuid);
            }

            return deliveryPhase;
        }

        private static IList<invln_homesindeliveryphase> MapHomesInDeliveryPhase(DeliveryPhaseDto deliveryPhaseDto)
        {
            if (deliveryPhaseDto.numberOfHomes != null && deliveryPhaseDto.numberOfHomes.Count > 0)
            {
                var homesInDeliveryPhase = new List<invln_homesindeliveryphase>();
                foreach (var homeInPhase in deliveryPhaseDto.numberOfHomes)
                {
                    homesInDeliveryPhase.Add(new invln_homesindeliveryphase
                    {
                        invln_hometypelookup = new EntityReference(invln_scheme.EntityLogicalName, Guid.Parse(homeInPhase.Key)),
                        invln_numberofhomes = homeInPhase.Value
                    });
                }

                return homesInDeliveryPhase;
            }

            return null;
        }

        private static string MapTypeOfHome(Nullable<bool> value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value ? "rehab" : "newBuild";
        }

        private static Nullable<bool> MapTypeOfHome(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value == "rehab";
        }

        private static string MapYesNo(Nullable<bool> value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value ? "yes" : "no";
        }

        private static Nullable<bool> MapYesNo(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value == "yes";
        }

        private static OptionSetValue MapNullableIntToOptionSetValue(int? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new OptionSetValue(valueToMap.Value);
            }
            return null;
        }

        private static Money MapNullableDecimalToMoney(decimal? valueToMap)
        {
            if (valueToMap.HasValue)
            {
                return new Money(valueToMap.Value);
            }
            return null;
        }
    }
}
