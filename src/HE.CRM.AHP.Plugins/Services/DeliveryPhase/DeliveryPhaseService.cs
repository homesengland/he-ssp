using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Services.DeliveryPhase
{
    public class DeliveryPhaseService : CrmService, IDeliveryPhaseService
    {
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

        private readonly IHomesInDeliveryPhaseRepository _homesInDeliveryPhaseRepository;

        private readonly IContactRepository _contactRepository;

        private readonly IAhpApplicationRepository _ahpApplicationRepository;

        private readonly IProgrammeRepository _ahpProgrammeRepository;

        private readonly IMilestoneFrameworkItemRepository _ahpMilestoneFrameworkItemRepository;

        public DeliveryPhaseService(CrmServiceArgs args) : base(args)
        {
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _homesInDeliveryPhaseRepository = CrmRepositoriesFactory.Get<IHomesInDeliveryPhaseRepository>();
            _ahpProgrammeRepository = CrmRepositoriesFactory.Get<IProgrammeRepository>();
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
                // var programme = _ahpProgrammeRepository.GetById(application.invln_programmelookup.Id, [invln_programme.Fields.Id]);
                TracingService.Trace($"Get Milestones");
                if (application.invln_programmelookup == null)
                {
                    TracingService.Trace($"No programme - Finish function");
                    return Guid.Empty;
                }
                var milestones = _ahpMilestoneFrameworkItemRepository.
                    GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, application.invln_programmelookup.Id).ToList();
                if (application.invln_noofhomes == null || application.invln_noofhomes == 0)
                {
                    return Guid.Empty;
                }
                TracingService.Trace($"{organisationGuid}");
                if (string.IsNullOrEmpty(devlieryPhaseDto.id) &&
                   _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisation(applicationGuid, organisationGuid))
                {
                    CalculateFundingOnPhaseCreate(application, deliveryPhaseMapped, milestones);
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
                    deliveryPhaseToUpdateOrCreate.Id = deliveryPhaseGuid;
                    if (deliveryPhaseMapped.invln_NoofHomes != null)
                    {
                        CalculateFunding(application, deliveryPhaseMapped, milestones, deliveryPhaseToUpdateOrCreate);
                    }
                    _deliveryPhaseRepository.Update(deliveryPhaseToUpdateOrCreate);
                    DeleteHomesFromDeliveryPhase(deliveryPhaseGuid);
                    SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseGuid);
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return deliveryPhaseToUpdateOrCreate.Id;
                }
            }
            return Guid.Empty;
        }

        public void CalculateFundingOnPhaseCreate(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones)
        {
            if (deliveryPhaseMapped.invln_NoofHomes != null)
            {
                var numberOfHouseApplication = application.invln_noofhomes.Value;
                var numberOfHousePhase = deliveryPhaseMapped.invln_NoofHomes.Value;
                var fundingRequired = application.invln_fundingrequired.Value;
                var acquisitionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value / 100;
                var startOnSitePercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value / 100;
                var completionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value / 100;
                deliveryPhaseMapped.invln_AcquisitionValue = new Money((fundingRequired / (decimal)numberOfHouseApplication) * (decimal)numberOfHousePhase * acquisitionPercentageValue);
                deliveryPhaseMapped.invln_AcquisitionPercentageValue = acquisitionPercentageValue;
                deliveryPhaseMapped.invln_StartOnSiteValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * startOnSitePercentageValue);
                deliveryPhaseMapped.invln_StartOnSitePercentageValue = startOnSitePercentageValue;
                deliveryPhaseMapped.invln_CompletionValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * completionPercentageValue);
                deliveryPhaseMapped.invln_CompletionPercentageValue = completionPercentageValue;
            }
        }

        public void CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate)
        {
            var numberOfHouseApplication = application.invln_noofhomes.Value;
            var numberOfHousePhase = deliveryPhaseMapped.invln_NoofHomes.Value;
            var fundingRequired = application.invln_fundingrequired.Value;
            var acquisitionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value / 100;
            var startOnSitePercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value / 100;
            var completionPercentageValue = milestones.FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value / 100;
            deliveryPhaseToUpdateOrCreate.invln_AcquisitionValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * acquisitionPercentageValue);
            deliveryPhaseToUpdateOrCreate.invln_AcquisitionPercentageValue = acquisitionPercentageValue;
            deliveryPhaseToUpdateOrCreate.invln_StartOnSiteValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * startOnSitePercentageValue);
            deliveryPhaseToUpdateOrCreate.invln_StartOnSitePercentageValue = startOnSitePercentageValue;
            deliveryPhaseToUpdateOrCreate.invln_CompletionValue = new Money((fundingRequired / numberOfHouseApplication) * numberOfHousePhase * completionPercentageValue);
            deliveryPhaseToUpdateOrCreate.invln_CompletionPercentageValue = completionPercentageValue;
            deliveryPhaseToUpdateOrCreate.invln_NoofHomes = numberOfHousePhase;
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
