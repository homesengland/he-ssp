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
        private readonly IAccountRepository _accountRepository;

        public DeliveryPhaseService(CrmServiceArgs args) : base(args)
        {
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _homesInDeliveryPhaseRepository = CrmRepositoriesFactory.Get<IHomesInDeliveryPhaseRepository>();
            _ahpMilestoneFrameworkItemRepository = CrmRepositoriesFactory.Get<IMilestoneFrameworkItemRepository>();
            _accountRepository = CrmRepositoriesFactory.Get<IAccountRepository>();
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
                                              invln_scheme.Fields.invln_programmelookup,
                                              invln_scheme.Fields.invln_organisationid});

                TracingService.Trace("Deserialize and Map imput data");
                var devlieryPhaseDto = JsonSerializer.Deserialize<DeliveryPhaseDto>(deliveryPhase);
                var deliveryPhaseMapped = DeliveryPhaseMapper.MapDtoToRegularEntity(devlieryPhaseDto, applicationId);

                TracingService.Trace($"Get Contact by externalUserId:{userId}");
                var contact = _contactRepository.GetContactViaExternalId(userId);
                TracingService.Trace($"Get Milestones");
                TracingService.Trace($"{organisationGuid}");
                if (string.IsNullOrEmpty(devlieryPhaseDto.id) &&
                   _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisation(applicationGuid, organisationGuid))
                {
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

        public invln_DeliveryPhase CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, bool resetMilestone, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate = null)
        {
            TracingService.Trace($"Calculation");
            if (milestones.Count == 0)
            {
                TracingService.Trace($"No milestones");
                return null;
            }

            if (application.invln_noofhomes == null || deliveryPhaseMapped.invln_NoofHomes == null ||
                application.invln_fundingrequired == null)
            {
                return null;
            }
            var account = _accountRepository.GetById(application.invln_organisationid, Account.Fields.invln_UnregisteredBody);
            TracingService.Trace($"numberOfHouseApplication: {application.invln_noofhomes.Value}");

            var numberOfHouseApplication = application.invln_noofhomes.Value;
            TracingService.Trace($"numberOfHousePhase: {deliveryPhaseMapped.invln_NoofHomes.Value}");

            var numberOfHousePhase = deliveryPhaseMapped.invln_NoofHomes.Value;
            TracingService.Trace($"fundingRequired: {application.invln_fundingrequired.Value}");

            var fundingRequired = application.invln_fundingrequired.Value;

            var fundingForPhase = (fundingRequired / numberOfHouseApplication) * numberOfHousePhase;

            var acquisitionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value / 100m;
            var startOnSitePercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value / 100m;
            var completionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value / 100m;

            TracingService.Trace($"milestone: {completionPercentageValue}");

            if ((account.invln_UnregisteredBody == true || account.invln_UnregisteredBody == null)
                || (deliveryPhaseMapped.invln_buildactivitytype != null && deliveryPhaseMapped.invln_buildactivitytype.Value == (int)invln_NewBuildActivityType.OffTheShelf)
                || (deliveryPhaseMapped.invln_rehabactivitytype != null && deliveryPhaseMapped.invln_rehabactivitytype.Value == (int)invln_RehabActivityType.ExistingSatisfactory))
            {
                acquisitionPercentageValue = 0m;
                startOnSitePercentageValue = 0m;
                completionPercentageValue = 1m;
            }
            else
            {
                if (!resetMilestone)
                {
                    if (!(deliveryPhaseMapped.invln_AcquisitionPercentageValue == 0m && deliveryPhaseMapped.invln_StartOnSitePercentageValue == 0m
                        && deliveryPhaseMapped.invln_CompletionPercentageValue == 100m))
                    {
                        if (deliveryPhaseMapped.invln_AcquisitionPercentageValue != null)
                        {
                            acquisitionPercentageValue = (int)deliveryPhaseMapped.invln_AcquisitionPercentageValue == (int)(acquisitionPercentageValue * 100) ? acquisitionPercentageValue : deliveryPhaseMapped.invln_AcquisitionPercentageValue.Value / 100m;
                        }
                        TracingService.Trace($"invln_AcquisitionPercentageValue:{deliveryPhaseMapped.invln_AcquisitionPercentageValue} {acquisitionPercentageValue}");
                        if (deliveryPhaseMapped.invln_StartOnSitePercentageValue != null)
                        {
                            startOnSitePercentageValue = (int)deliveryPhaseMapped.invln_StartOnSitePercentageValue == (int)(startOnSitePercentageValue * 100) ? startOnSitePercentageValue : deliveryPhaseMapped.invln_StartOnSitePercentageValue.Value / 100m;
                        }
                        TracingService.Trace($"invln_StartOnSitePercentageValue:{deliveryPhaseMapped.invln_StartOnSitePercentageValue} {startOnSitePercentageValue}");
                        if (deliveryPhaseMapped.invln_CompletionPercentageValue != null)
                        {
                            completionPercentageValue = (int)deliveryPhaseMapped.invln_CompletionPercentageValue == (int)completionPercentageValue * 100 ? completionPercentageValue : deliveryPhaseMapped.invln_CompletionPercentageValue.Value / 100m;
                        }
                        TracingService.Trace($"invln_CompletionPercentageValue:{deliveryPhaseMapped.invln_CompletionPercentageValue} {completionPercentageValue}");
                    }
                }
            }
            if (deliveryPhaseToUpdateOrCreate == null)
            {
                TracingService.Trace($"CalculateFundings deliveryPhaseToUpdateOrCreate == null");
                CalculateFundings(deliveryPhaseMapped, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase, application.invln_fundingrequired.Value);
                return deliveryPhaseMapped;
            }
            else
            {
                TracingService.Trace($"CalculateFundings");
                CalculateFundings(deliveryPhaseToUpdateOrCreate, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase, application.invln_fundingrequired.Value);
                return deliveryPhaseToUpdateOrCreate;
            }
        }

        private void CalculateFundings(invln_DeliveryPhase deliveryPhase, decimal acquisitionPercentageValue, decimal startOnSitePercentageValue, decimal completionPercentageValue, decimal fundingForPhase, decimal grandRequested)
        {
            TracingService.Trace($"acquisitionPercentageValue:{acquisitionPercentageValue}");
            TracingService.Trace($"startOnSitePercentageValue:{startOnSitePercentageValue}");
            TracingService.Trace($"completionPercentageValue:{completionPercentageValue}");
            fundingForPhase = Math.Round(fundingForPhase, 0, MidpointRounding.AwayFromZero);
            deliveryPhase.invln_AcquisitionPercentageValue = acquisitionPercentageValue * 100;
            deliveryPhase.invln_StartOnSitePercentageValue = startOnSitePercentageValue * 100;
            deliveryPhase.invln_CompletionPercentageValue = completionPercentageValue * 100;
            deliveryPhase.invln_AcquisitionValue = new Money(fundingForPhase * acquisitionPercentageValue);
            deliveryPhase.invln_StartOnSiteValue = new Money(fundingForPhase * startOnSitePercentageValue);
            deliveryPhase.invln_CompletionValue = new Money(fundingForPhase * completionPercentageValue);
            CalculateFieldValue(deliveryPhase.invln_AcquisitionValue.Value, deliveryPhase.invln_StartOnSiteValue.Value, deliveryPhase.invln_CompletionValue.Value,
                                acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue,
                                deliveryPhase, fundingForPhase);
            ValidateGrantRequest(deliveryPhase.invln_Application.Id, grandRequested);
            TracingService.Trace("End Of Calculation");
        }

        private void ValidateGrantRequest(Guid aplicationId, decimal grandRequested)
        {
            var deliveryPhases = _deliveryPhaseRepository.GetByAttribute(invln_DeliveryPhase.Fields.invln_Application, aplicationId,
                new string[] { invln_DeliveryPhase.Fields.invln_sumofcalculatedfounds, invln_DeliveryPhase.Fields.invln_CompletionValue });
            var sumOfFounds = deliveryPhases.Sum(x => x.invln_sumofcalculatedfounds.Value);
            if (sumOfFounds != grandRequested)
            {
                TracingService.Trace("Adjust last deliveryphase");
                var lastDeliveryPhase = deliveryPhases.OrderByDescending(x => x.invln_completionmilestoneclaimdate).FirstOrDefault();
                lastDeliveryPhase.invln_CompletionValue = new Money(lastDeliveryPhase.invln_CompletionValue.Value + (sumOfFounds - grandRequested));
                lastDeliveryPhase.invln_sumofcalculatedfounds = new Money(lastDeliveryPhase.invln_sumofcalculatedfounds.Value + (sumOfFounds - grandRequested));
            }
        }

        private void CalculateFieldValue(decimal acquisition, decimal startOnSite, decimal completion,
                                            decimal acquisitionPer, decimal startOnSitePer, decimal completionPer,
                                            invln_DeliveryPhase deliveryPhase, decimal fundingForPhase)
        {
            if ((int)(acquisitionPer + startOnSitePer + completionPer) == 100)
            {
                var leftOver = fundingForPhase - (acquisition + startOnSite + completion);
                if (leftOver > 0 && (leftOver < fundingForPhase * 0.01m || leftOver < 1))
                {
                    deliveryPhase.invln_CompletionValue.Value += leftOver;
                }
            }
            deliveryPhase.invln_sumofcalculatedfounds = new Money(acquisition + startOnSite + deliveryPhase.invln_CompletionValue.Value);
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