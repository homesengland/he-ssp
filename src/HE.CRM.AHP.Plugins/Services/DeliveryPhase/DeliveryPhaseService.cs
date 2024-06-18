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
            Logger.Trace($"{nameof(DeliveryPhaseService)}.{nameof(SetDeliveryPhase)}");
            if (Guid.TryParse(applicationId, out var applicationGuid) && Guid.TryParse(organisationId, out var organisationGuid))
            {
                Logger.Trace("Get application");
                var application = _ahpApplicationRepository.GetById(
                    applicationGuid,
                    new string[] {
                        invln_scheme.Fields.invln_noofhomes,
                        invln_scheme.Fields.invln_fundingrequired,
                        invln_scheme.Fields.invln_programmelookup,
                        invln_scheme.Fields.invln_organisationid
                    });

                Logger.Trace("Deserialize and Map input data");
                var devlieryPhaseDto = JsonSerializer.Deserialize<DeliveryPhaseDto>(deliveryPhase);
                var deliveryPhaseMapped = DeliveryPhaseMapper.MapDtoToRegularEntity(devlieryPhaseDto, applicationId);

                Logger.Trace($"Get Contact by invln_externalid: {userId}");
                var contact = _contactRepository.GetContactViaExternalId(userId);

                Logger.Trace($"Get Milestones");
                var milestones = new List<invln_milestoneframeworkitem>();
                if (application.invln_programmelookup != null)
                {
                    milestones = _ahpMilestoneFrameworkItemRepository.GetByAttribute(
                        invln_milestoneframeworkitem.Fields.invln_programmeId,
                        application.invln_programmelookup.Id,
                        new string[] {
                            invln_milestoneframeworkitem.Fields.invln_milestone,
                            invln_milestoneframeworkitem.Fields.invln_percentagepaidonmilestone
                        }).ToList();
                }

                Logger.Trace($"{organisationGuid}");
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
                        // to remove !
                        CalculateFunding(application, deliveryPhaseMapped, milestones, deliveryPhaseToUpdateOrCreate);
                    }
                    TracingService.Trace($"Update DF");
                    _deliveryPhaseRepository.Update(deliveryPhaseToUpdateOrCreate);
                    //DeleteHomesFromDeliveryPhase(deliveryPhaseGuid);
                    //SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseGuid);
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);

                    // update: Homes in Delivery Phase
                    //var homesToRemove = deliveryPhaseToUpdateOrCreate.invln_invln_homesindeliveryphase_deliveryphasel
                    //    .Select(x => x.invln_invln_homesindeliveryphase_hometypelookup.Id).Any(x => devlieryPhaseDto.numberOfHomes.ContainsKey(x.ToString()));

                    var homesInDeliveryPhaseToUpdate = _homesInDeliveryPhaseRepository.GetHomesInDeliveryPhase(deliveryPhaseGuid);
                    foreach (var home in homesInDeliveryPhaseToUpdate)
                    {
                        Logger.Trace($"homeId: {home.Id}, numberofhomes: {home.invln_numberofhomes}, home type: {home.invln_hometypelookup.Id}");
                    }

                    foreach (var homesOnDeliveryPhase in deliveryPhaseMapped.invln_invln_homesindeliveryphase_deliveryphasel)
                    {
                        Logger.Trace($"numberofhomes: {homesOnDeliveryPhase.invln_numberofhomes}, home type: {homesOnDeliveryPhase.invln_hometypelookup.Id}");


                        var homesOnDeliveryPhaseToUpdate = homesInDeliveryPhaseToUpdate.Single(x => x.invln_hometypelookup.Id == homesOnDeliveryPhase.invln_hometypelookup.Id);
                        if (homesOnDeliveryPhaseToUpdate.invln_numberofhomes != homesOnDeliveryPhase.invln_numberofhomes)
                        {
                            Logger.Trace($"Update invln_homesindeliveryphase.id: {homesOnDeliveryPhaseToUpdate.Id}, nohomes: {homesOnDeliveryPhase.invln_numberofhomes}");
                            _homesInDeliveryPhaseRepository.Update(
                                new invln_homesindeliveryphase()
                                {
                                    Id = homesOnDeliveryPhaseToUpdate.Id,
                                    invln_numberofhomes = homesOnDeliveryPhase.invln_numberofhomes
                                });
                        }
                    }

                    return deliveryPhaseToUpdateOrCreate.Id;
                }
            }
            return Guid.Empty;
        }

        public void CalculateFunding(invln_scheme application, invln_DeliveryPhase deliveryPhaseMapped, List<invln_milestoneframeworkitem> milestones, invln_DeliveryPhase deliveryPhaseToUpdateOrCreate = null)
        {
            Logger.Trace($"Calculation");
            if (milestones.Count == 0)
            {
                return;
            }

            if (application.invln_noofhomes == null || deliveryPhaseMapped.invln_NoofHomes == null ||
                application.invln_fundingrequired == null)
            {
                return;
            }

            var account = _accountRepository.GetById(application.invln_organisationid, Account.Fields.invln_UnregisteredBody);
            TracingService.Trace($"numberOfHouseApplication: {application.invln_noofhomes.Value}");
            var numberOfHouseApplication = application.invln_noofhomes.Value;
            TracingService.Trace($"numberOfHousePhase: {deliveryPhaseMapped.invln_NoofHomes.Value}");
            var numberOfHousePhase = deliveryPhaseMapped.invln_NoofHomes.Value;
            TracingService.Trace($"fundingRequired: {application.invln_fundingrequired.Value}");
            var fundingRequired = application.invln_fundingrequired.Value;
            var acquisitionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.Acquisition).invln_percentagepaidonmilestone.Value;
            var startOnSitePercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.SoS).invln_percentagepaidonmilestone.Value;
            var completionPercentageValue = milestones
                    .FirstOrDefault(x => x.invln_milestone.Value == (int)invln_Milestone.PC).invln_percentagepaidonmilestone.Value;
            var fundingForPhase = fundingRequired / numberOfHouseApplication * numberOfHousePhase;

            if (account.invln_UnregisteredBody == true || account.invln_UnregisteredBody == null
                || (deliveryPhaseMapped.invln_buildactivitytype != null && deliveryPhaseMapped.invln_buildactivitytype.Value == (int)invln_NewBuildActivityType.OffTheShelf)
                || (deliveryPhaseMapped.invln_rehabactivitytype != null && deliveryPhaseMapped.invln_rehabactivitytype.Value == (int)invln_RehabActivityType.ExistingSatisfactory))
            {
                deliveryPhaseToUpdateOrCreate.invln_CompletionValue = new Money(fundingRequired / numberOfHouseApplication * numberOfHousePhase);
                deliveryPhaseToUpdateOrCreate.invln_StartOnSiteValue = new Money(0);
                deliveryPhaseToUpdateOrCreate.invln_AcquisitionValue = new Money(0);
                deliveryPhaseToUpdateOrCreate.invln_CompletionPercentageValue = 100;
                deliveryPhaseToUpdateOrCreate.invln_StartOnSitePercentageValue = 0;
                deliveryPhaseToUpdateOrCreate.invln_AcquisitionPercentageValue = 0;
                deliveryPhaseToUpdateOrCreate.invln_sumofcalculatedfounds = new Money(fundingRequired / numberOfHouseApplication * numberOfHousePhase);
            }
            else
            {
                if (deliveryPhaseToUpdateOrCreate == null)
                {
                    TracingService.Trace($"CalculateFundings deliveryPhaseToUpdateOrCreate == null");
                    CalculateFundings(deliveryPhaseMapped, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase);
                }
                else
                {
                    TracingService.Trace($"CalculateFundings");
                    CalculateFundings(deliveryPhaseToUpdateOrCreate, acquisitionPercentageValue, startOnSitePercentageValue, completionPercentageValue, fundingForPhase);
                }
            }
        }

        private void CalculateFundings(invln_DeliveryPhase deliveryPhase, decimal acquisitionPercentageValue, decimal startOnSitePercentageValue, decimal completionPercentageValue, decimal fundingForPhase)
        {
            Logger.Trace("Check Delivery Phase");
            invln_DeliveryPhase df = null;
            if (deliveryPhase != null)
            {
                Logger.Trace("Get values from Delivery Phase");
                df = _deliveryPhaseRepository.GetById(deliveryPhase.Id,
                                    new string[] { invln_DeliveryPhase.Fields.invln_AcquisitionPercentageValue,
                                               invln_DeliveryPhase.Fields.invln_StartOnSitePercentageValue,
                                               invln_DeliveryPhase.Fields.invln_CompletionPercentageValue,
                                               invln_DeliveryPhase.Fields.invln_AcquisitionValue,
                                               invln_DeliveryPhase.Fields.invln_StartOnSiteValue,
                                               invln_DeliveryPhase.Fields.invln_CompletionValue,
                                    });
            }
            if (deliveryPhase.invln_AcquisitionPercentageValue == null && deliveryPhase.invln_StartOnSitePercentageValue == null && deliveryPhase.invln_CompletionPercentageValue == null)
            {
                deliveryPhase.invln_AcquisitionPercentageValue = df.invln_AcquisitionPercentageValue == acquisitionPercentageValue ? acquisitionPercentageValue : df.invln_AcquisitionPercentageValue;
                deliveryPhase.invln_StartOnSitePercentageValue = df.invln_StartOnSitePercentageValue == startOnSitePercentageValue ? startOnSitePercentageValue : df.invln_StartOnSitePercentageValue;
                deliveryPhase.invln_CompletionPercentageValue = df.invln_CompletionPercentageValue == completionPercentageValue ? completionPercentageValue : df.invln_CompletionPercentageValue;
                deliveryPhase.invln_AcquisitionValue = new Money(fundingForPhase * deliveryPhase.invln_AcquisitionPercentageValue.Value / 100);
                deliveryPhase.invln_StartOnSiteValue = new Money(fundingForPhase * deliveryPhase.invln_StartOnSitePercentageValue.Value / 100);
                deliveryPhase.invln_CompletionValue = new Money(fundingForPhase * deliveryPhase.invln_CompletionPercentageValue.Value / 100);
                deliveryPhase.invln_sumofcalculatedfounds = new Money(fundingForPhase);
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
                    deliveryPhase.invln_AcquisitionValue = new Money(fundingForPhase * deliveryPhase.invln_AcquisitionPercentageValue.Value / 100);
                    CalculateFieldValue(deliveryPhase.invln_AcquisitionValue.Value, df.invln_StartOnSiteValue.Value, df.invln_CompletionValue.Value,
                                        deliveryPhase.invln_AcquisitionPercentageValue.Value, df.invln_StartOnSitePercentageValue.Value, df.invln_CompletionPercentageValue.Value,
                                        deliveryPhase, fundingForPhase);
                }
                if (deliveryPhase.invln_StartOnSitePercentageValue != null)
                {
                    deliveryPhase.invln_StartOnSiteValue = new Money(fundingForPhase * deliveryPhase.invln_StartOnSitePercentageValue.Value / 100);
                    CalculateFieldValue(df.invln_AcquisitionValue.Value, deliveryPhase.invln_StartOnSiteValue.Value, df.invln_CompletionValue.Value,
                                        df.invln_AcquisitionPercentageValue.Value, deliveryPhase.invln_StartOnSitePercentageValue.Value, df.invln_CompletionPercentageValue.Value,
                                        deliveryPhase, fundingForPhase);
                }
                if (deliveryPhase.invln_CompletionPercentageValue != null)
                {
                    deliveryPhase.invln_CompletionValue = new Money(fundingForPhase * deliveryPhase.invln_CompletionPercentageValue.Value / 100);
                    CalculateFieldValue(df.invln_AcquisitionValue.Value, df.invln_StartOnSiteValue.Value, deliveryPhase.invln_CompletionValue.Value,
                                        df.invln_AcquisitionPercentageValue.Value, df.invln_StartOnSitePercentageValue.Value, deliveryPhase.invln_CompletionPercentageValue.Value,
                                        deliveryPhase, fundingForPhase);
                }
            }
            TracingService.Trace("End Of Calculation");
        }

        private void CalculateFieldValue(decimal acquisition, decimal startOnSite, decimal completion,
                                            decimal acquisitionPer, decimal startOnSitePer, decimal completionPer,
                                            invln_DeliveryPhase deliveryPhase, decimal fundingForPhase)
        {
            if (acquisitionPer + startOnSitePer + completionPer == 100)
            {
                var leftOver = fundingForPhase - (acquisition + startOnSite + completion);
                if (leftOver > 0 && (leftOver < fundingForPhase * 0.01m || leftOver < 1))
                {
                    deliveryPhase.invln_CompletionValue.Value += leftOver;
                }
            }
            deliveryPhase.invln_sumofcalculatedfounds = new Money(acquisition + startOnSite + completion);
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
