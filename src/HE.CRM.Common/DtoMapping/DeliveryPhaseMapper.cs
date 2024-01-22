using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                //createdByExternalUserName { get; set; }
                //typeOfHomes = deliveryPhase.home
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
                //requiresAdditionalPayments =deliveryPhase.
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

        public static invln_DeliveryPhase MapDtoToRegularEntity(DeliveryPhaseDto deliveryPhaseDto)
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

            };

            if (deliveryPhaseDto.id != null)
            {
                deliveryPhase.Id = new Guid(deliveryPhaseDto.id);
            }

            return deliveryPhase;
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
