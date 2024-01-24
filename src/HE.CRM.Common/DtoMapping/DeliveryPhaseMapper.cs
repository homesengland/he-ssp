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
        public static DeliveryPhaseDto MapRegularEntityToDto(invln_DeliveryPhase deliveryPhase)
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
            };

            if (deliveryPhase.Id != null)
            {
                deliveryPhaseDto.id = deliveryPhase.Id.ToString();
            }

            if (deliveryPhase.invln_Application?.Id != null)
            {
                deliveryPhaseDto.applicationId = deliveryPhase.invln_Application.Id.ToString();
            }

            if (deliveryPhase.invln_invln_homesindeliveryphase_deliveryphasel != null && deliveryPhase.invln_invln_homesindeliveryphase_deliveryphasel.Count() > 0)
            {
                foreach (var homeInPhase in deliveryPhase.invln_invln_homesindeliveryphase_deliveryphasel)
                {
                    deliveryPhaseDto.numberOfHomes.Add(homeInPhase.invln_hometypelookup?.Id.ToString(), homeInPhase.invln_numberofhomes);
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
            };

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

            return Array.Empty<invln_homesindeliveryphase>();
        }

        private static string MapTypeOfHome(Nullable<bool> value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value ? "newBuild" : "rehab";
        }

        private static Nullable<bool> MapTypeOfHome(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value == "newBuild";
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
    }
}
