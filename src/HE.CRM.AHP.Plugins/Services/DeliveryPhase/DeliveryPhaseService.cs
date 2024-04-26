using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.AHP.Plugins.Services.DeliveryPhase
{
    public class DeliveryPhaseService : CrmService, IDeliveryPhaseService
    {
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
        private readonly IHomesInDeliveryPhaseRepository _homesInDeliveryPhaseRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IMilestoneFrameworkItemRepository _ahpMilestoneFrameworkItemRepository;

        public DeliveryPhaseService(CrmServiceArgs args) : base(args)
        {
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _homesInDeliveryPhaseRepository = CrmRepositoriesFactory.Get<IHomesInDeliveryPhaseRepository>();
            _ahpMilestoneFrameworkItemRepository = CrmRepositoriesFactory.Get<IMilestoneFrameworkItemRepository>();
        }

        public void DeleteDeliveryPhase(string applicationId, string organisationId, string deliveryPhaseId, string externalUserId)
        {
            if (Guid.TryParse(deliveryPhaseId, out var deliveryPhaseGuid) && Guid.TryParse(organisationId, out var organisationGuid) &&
                Guid.TryParse(applicationId, out var applicationGuid))
            {
                if (_deliveryPhaseRepository.CheckIfGivenDeliveryPhaseIsAssignedToGivenOrganisationAndApplication(deliveryPhaseGuid, organisationGuid, applicationGuid))
                {
                    var contact = _contactRepository.GetContactViaExternalId(externalUserId);
                    _deliveryPhaseRepository.Delete(new invln_DeliveryPhase()
                    {
                        Id = deliveryPhaseGuid
                    });
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                }
            }
        }

        public DeliveryPhaseDto GetDeliveryPhase(string applicationId, string organizationId, string externalUserId, string deliveryPhaseId, string fieldsToRetrieve = null)
        {
            TracingService.Trace("Start GetDeliveryPhase");
            DeliveryPhaseDto deliveryPhaseDto = null;
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var deliveryPhase = _deliveryPhaseRepository.GetDeliveryPhaseForNullableUserAndOrganisationByIdAndApplicationId(deliveryPhaseId, applicationId, externalUserId, organizationId, attributes);
            if (deliveryPhase != null)
            {
                var homesInDeliveryPhase = _homesInDeliveryPhaseRepository.GetHomesInDeliveryPhase(Guid.Parse(deliveryPhaseId));
                deliveryPhaseDto = DeliveryPhaseMapper.MapRegularEntityToDto(deliveryPhase, homesInDeliveryPhase);
            }

            return deliveryPhaseDto;
        }

        public List<DeliveryPhaseDto> GetDeliveryPhases(string applicationId, string organizationId, string externalUserId, string fieldsToRetrieve = null)
        {
            var deliveryPhasesDto = new List<DeliveryPhaseDto>();
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var deliveryPhases = _deliveryPhaseRepository.GetDeliveryPhasesForNullableUserAndOrganisationRelatedToApplication(applicationId, externalUserId, organizationId, attributes);
            {
                foreach (var deliveryPhase in deliveryPhases)
                {
                    var homesInDeliveryPhase = _homesInDeliveryPhaseRepository.GetHomesInDeliveryPhase(deliveryPhase.Id);
                    deliveryPhasesDto.Add(DeliveryPhaseMapper.MapRegularEntityToDto(deliveryPhase, homesInDeliveryPhase));
                }
            }
            return deliveryPhasesDto;
        }

        public Guid SetDeliveryPhase(string deliveryPhase, string userId, string organisationId, string applicationId, string fieldsToSet = null)
        {
            TracingService.Trace("Start SetDeliveryPhase function");
            if (Guid.TryParse(applicationId, out var applicationGuid) && Guid.TryParse(organisationId, out var organisationGuid))
            {
                TracingService.Trace("Get Application");
                var application = _ahpApplicationRepository.
                    GetById(applicationGuid, new string[] { invln_scheme.Fields.invln_noofhomes,
                                              invln_scheme.Fields.invln_fundingrequired,
                                              invln_scheme.Fields.invln_programmelookup });

                TracingService.Trace("Deserialize and Map imput data");
                var devlieryPhaseDto = JsonSerializer.Deserialize<DeliveryPhaseDto>(deliveryPhase);
                var deliveryPhaseMapped = DeliveryPhaseMapper.MapDtoToRegularEntity(devlieryPhaseDto, applicationId);

                TracingService.Trace($"Get Contact by externalUserId:{userId}");
                var contact = _contactRepository.GetContactViaExternalId(userId);
                TracingService.Trace($"Get Milestones");
                var milestones = new List<invln_milestoneframeworkitem>();
                if (application.invln_programmelookup != null)
                {
                    milestones = _ahpMilestoneFrameworkItemRepository.
                    GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, application.invln_programmelookup.Id).ToList();
                }
                TracingService.Trace($"{organisationGuid}");
                if (string.IsNullOrEmpty(devlieryPhaseDto.id) &&
                   _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisation(applicationGuid, organisationGuid))
                {
                    if (deliveryPhaseMapped.invln_NoofHomes != null)
                    {
                        CalculateFunding(application, deliveryPhaseMapped, milestones, null);
                    }
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    var deliveryPhaseId = _deliveryPhaseRepository.Create(deliveryPhaseMapped);
                    SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseId);
                    return deliveryPhaseId;
                }
                else if (Guid.TryParse(devlieryPhaseDto.id, out var deliveryPhaseGuid))
                {
                    invln_DeliveryPhase deliveryPhaseToUpdateOrCreate;
                    if (!string.IsNullOrEmpty(fieldsToSet))
                    {
                        var fields = fieldsToSet.Split(',');
                        deliveryPhaseToUpdateOrCreate = new invln_DeliveryPhase();
                        foreach (var field in fields)
                        {
                            TracingService.Trace($"field name {field}");
                            if (deliveryPhaseMapped.Contains(field))
                            {
                                TracingService.Trace($"contains");
                                deliveryPhaseToUpdateOrCreate[field] = deliveryPhaseMapped[field];
                            }
                        }
                    }
                    else
                    {
                        deliveryPhaseToUpdateOrCreate = deliveryPhaseMapped;
                    }
                    TracingService.Trace($"After mapping");
                    deliveryPhaseToUpdateOrCreate.Id = deliveryPhaseGuid;
                    if (deliveryPhaseMapped.invln_NoofHomes != null)
                    {
                        CalculateFunding(application, deliveryPhaseMapped, milestones, deliveryPhaseToUpdateOrCreate);
                    }
                    TracingService.Trace($"Update DF");
                    _deliveryPhaseRepository.Update(deliveryPhaseToUpdateOrCreate);
                    DeleteHomesFromDeliveryPhase(deliveryPhaseGuid);
                    SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseGuid);
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return deliveryPhaseToUpdateOrCreate.Id;
                }
            }
            return Guid.Empty;
        }

        public void CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate = null)
        {
            TracingService.Trace($"Calculation");
            if (milestones.Count == 0)
            {
                return;
            }

            if (application.invln_noofhomes == null || deliveryPhaseMapped.invln_NoofHomes == null ||
                application.invln_fundingrequired == null)
            {
                return;
            }

            TracingService.Trace($"numberOfHouseApplication: {application.invln_noofhomes.Value}");
            var numberOfHouseApplication = application.invln_noofhomes.Value;
            TracingService.Trace($"numberOfHousePhase: {deliveryPhaseMapped.invln_NoofHomes.Value}");
            var numberOfHousePhase = deliveryPhaseMapped.invln_NoofHomes.Value;
            TracingService.Trace($"fundingRequired: {application.invln_fundingrequired.Value}");
            var fundingRequired = application.invln_fundingrequired.Value;
            var acquisitionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value / 100;
            var startOnSitePercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value / 100;
            var completionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value / 100;
            var fundingForPhase = (fundingRequired / numberOfHouseApplication) * numberOfHousePhase;
            if (deliveryPhaseToUpdateOrCreate == null)
            {
                TracingService.Trace($"CalculateFundings");
                CalculateFundings(deliveryPhaseMapped, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase);
            }
            else
            {
                TracingService.Trace($"CalculateFundings");
                CalculateFundings(deliveryPhaseToUpdateOrCreate, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase);
            }
        }

        private void CalculateFundings(invln_DeliveryPhase deliveryPhase, decimal acquisitionPercentageValue, decimal startOnSitePercentageValue, decimal completionPercentageValue, decimal fundingForPhase)
        {
            var df = _deliveryPhaseRepository.GetById(deliveryPhase.Id,
                                new string[] { invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue,
                                               invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue,
                                               invln_DeliveryPhase.Fields.invln_CompletionPercentageValue,
                                               invln_DeliveryPhase.Fields.invln_AcquisitionValue,
                                               invln_DeliveryPhase.Fields.invln_StartOnSiteValue,
                                               invln_DeliveryPhase.Fields.invln_CompletionValue,
                                });

            if (deliveryPhase.invln_AcquisitionPercentageValue == null && deliveryPhase.invln_StartOnSitePercentageValue == null && deliveryPhase.invln_CompletionPercentageValue == null)
            {
                deliveryPhase.invln_AcquisitionPercentageValue = acquisitionPercentageValue;
                deliveryPhase.invln_StartOnSitePercentageValue = startOnSitePercentageValue;
                deliveryPhase.invln_CompletionPercentageValue = completionPercentageValue;
                deliveryPhase.invln_AcquisitionValue = new Money(fundingForPhase * deliveryPhase.invln_AcquisitionPercentageValue.Value);
                deliveryPhase.invln_StartOnSiteValue = new Money(fundingForPhase * deliveryPhase.invln_StartOnSitePercentageValue.Value);
                deliveryPhase.invln_CompletionValue = new Money(fundingForPhase * deliveryPhase.invln_CompletionPercentageValue.Value);
                var leftOver = fundingForPhase
                    - (deliveryPhase.invln_AcquisitionValue.Value
                    + deliveryPhase.invln_StartOnSiteValue.Value
                    + deliveryPhase.invln_CompletionValue.Value);
                if (leftOver > 0 && (leftOver < fundingForPhase * 0.01m || leftOver < 1))
                {
                    deliveryPhase.invln_CompletionValue.Value += leftOver;
                }
                deliveryPhase.invln_sumofcalculatedfounds = new Money(deliveryPhase.invln_AcquisitionValue.Value
                                                            + deliveryPhase.invln_StartOnSiteValue.Value
                                                            + deliveryPhase.invln_CompletionValue.Value);
            }
            else
            {
                if (deliveryPhase.invln_AcquisitionPercentageValue != null)
                {
                    deliveryPhase.invln_AcquisitionValue = new Money(fundingForPhase * deliveryPhase.invln_AcquisitionPercentageValue.Value);
                    CalculateFieldValue(deliveryPhase, fundingForPhase, df);
                }
                if (deliveryPhase.invln_StartOnSitePercentageValue != null)
                {
                    deliveryPhase.invln_StartOnSiteValue = new Money(fundingForPhase * deliveryPhase.invln_StartOnSitePercentageValue.Value);
                    CalculateFieldValue(deliveryPhase, fundingForPhase, df);
                }
                if (deliveryPhase.invln_CompletionPercentageValue != null)
                {
                    deliveryPhase.invln_CompletionValue = new Money(fundingForPhase * deliveryPhase.invln_CompletionPercentageValue.Value);
                    CalculateFieldValue(deliveryPhase, fundingForPhase, df);
                }
                deliveryPhase.invln_sumofcalculatedfounds = new Money(deliveryPhase.invln_AcquisitionValue.Value
                                            + deliveryPhase.invln_StartOnSiteValue.Value
                                            + deliveryPhase.invln_CompletionValue.Value);
            }
        }

        private static void CalculateFieldValue(invln_DeliveryPhase deliveryPhase, decimal fundingForPhase, invln_DeliveryPhase df)
        {
            if (deliveryPhase.invln_AcquisitionPercentageValue + df.invln_StartOnSitePercentageValue + df.invln_CompletionPercentageValue == 1)
            {
                var leftOver = fundingForPhase
                                - (deliveryPhase.invln_AcquisitionValue.Value
                                    + df.invln_StartOnSiteValue.Value
                                    + df.invln_CompletionValue.Value);
                if (leftOver > 0 && (leftOver < fundingForPhase * 0.01m || leftOver < 1))
                {
                    deliveryPhase.invln_CompletionValue.Value += leftOver;
                }
            }
        }

        private void SetHomesinDeliveryPhase(Dictionary<string, int?> numberOfHomes, Guid deliveryPhaseId)
        {
            foreach (var numHome in numberOfHomes)
            {
                if (Guid.TryParse(numHome.Key, out var homeId))
                {
                    _homesInDeliveryPhaseRepository.Create(new invln_homesindeliveryphase()
                    {
                        invln_deliveryphaselookup = new EntityReference(invln_DeliveryPhase.EntityLogicalName, deliveryPhaseId),
                        invln_hometypelookup = new EntityReference(invln_HomeType.EntityLogicalName, homeId),
                        invln_numberofhomes = numHome.Value
                    });
                }
            }
        }

        private void DeleteHomesFromDeliveryPhase(Guid deliveryPhaseId)
        {
            var homesInDeliveryPhase = _homesInDeliveryPhaseRepository.GetHomesInDeliveryPhase(deliveryPhaseId);
            foreach (var home in homesInDeliveryPhase)
            {
                _homesInDeliveryPhaseRepository.Delete(home);
            }
        }

        private void UpdateApplicationModificationFields(Guid applicationId, Guid contactId)
        {
            var applicationToUpdate = new invln_scheme()
            {
                Id = applicationId,
                invln_lastexternalmodificationon = DateTime.UtcNow,
                invln_lastexternalmodificationby = new EntityReference(Contact.EntityLogicalName, contactId),
            };
            _ahpApplicationRepository.Update(applicationToUpdate);
        }

        private string GenerateFetchXmlAttributes(string fieldsToRetrieve)
        {
            var fields = fieldsToRetrieve.Split(',');
            var generatedAttribuesFetchXml = "";
            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    generatedAttribuesFetchXml += $"<attribute name=\"{field}\" />";
                }
            }
            return generatedAttribuesFetchXml;
        }
    }
}
