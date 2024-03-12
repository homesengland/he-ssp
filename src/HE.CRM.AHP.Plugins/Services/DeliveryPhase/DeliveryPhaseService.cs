using System;
using System.Collections.Generic;
using System.Linq;
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
                var application = _ahpApplicationRepository.GetById(applicationGuid, [invln_scheme.Fields.invln_noofhomes,
                                                                                        invln_scheme.Fields.invln_fundingfromopenmarkethomesonthisscheme
                                                                                        , invln_scheme.Fields.invln_programmelookup
                                                                                        ]);

                TracingService.Trace("Deserialize and Map imput data");
                var devlieryPhaseDto = JsonSerializer.Deserialize<DeliveryPhaseDto>(deliveryPhase);
                var deliveryPhaseMapped = DeliveryPhaseMapper.MapDtoToRegularEntity(devlieryPhaseDto, applicationId);

                TracingService.Trace($"Get Contact by externalUserId:{userId}");
                var contact = _contactRepository.GetContactViaExternalId(userId);


                var programme = _ahpProgrammeRepository.GetById(application.invln_programmelookup.Id, [invln_programme.Fields.Id]);
                var milestones = _ahpMilestoneFrameworkItemRepository.GetByAttribute(invln_milestoneframeworkitem.Fields.invln_programmeId, application.invln_programmelookup).ToList();
                var namberOfHouse = application.invln_noofhomes;
                if (namberOfHouse == null || namberOfHouse == 0)
                {
                    return Guid.Empty;
                }
                if (IsNewPhase(devlieryPhaseDto, applicationGuid, organisationGuid))
                {
                    var existingPhase = GetExistingPhase(applicationGuid);

                }
                else
                {

                }



                //if (string.IsNullOrEmpty(devlieryPhaseDto.id) &&
                //   _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisation(applicationGuid, organisationGuid))
                //{

                //    deliveryPhaseMapped.invln_AcquisitionValue = 0;
                //    deliveryPhaseMapped.invln_AcquisitionPercentageValue = 0;
                //    deliveryPhaseMapped.invln_StartOnSiteValue = ;
                //    deliveryPhaseMapped.invln_StartOnSitePercentageValue = ;
                //    deliveryPhaseMapped.invln_CompletionValue = ,
                //    deliveryPhaseMapped.invln_CompletionPercentageValue = ;
                //    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                //    var deliveryPhaseId = _deliveryPhaseRepository.Create(deliveryPhaseMapped);
                //    SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseId);
                //    return deliveryPhaseId;
                //}
                //else if (Guid.TryParse(devlieryPhaseDto.id, out var deliveryPhaseGuid))
                //{
                //    invln_DeliveryPhase deliveryPhaseToUpdateOrCreate;
                //    if (!string.IsNullOrEmpty(fieldsToSet))
                //    {
                //        var fields = fieldsToSet.Split(',');
                //        deliveryPhaseToUpdateOrCreate = new invln_DeliveryPhase();
                //        foreach (var field in fields)
                //        {
                //            TracingService.Trace($"field name {field}");
                //            if (deliveryPhaseMapped.Contains(field))
                //            {
                //                TracingService.Trace($"contains");
                //                deliveryPhaseToUpdateOrCreate[field] = deliveryPhaseMapped[field];
                //            }
                //        }
                //    }
                //    else
                //    {
                //        deliveryPhaseToUpdateOrCreate = deliveryPhaseMapped;
                //    }
                //    deliveryPhaseToUpdateOrCreate.Id = deliveryPhaseGuid;
                //    _deliveryPhaseRepository.Update(deliveryPhaseToUpdateOrCreate);
                //    DeleteHomesFromDeliveryPhase(deliveryPhaseGuid);
                //    SetHomesinDeliveryPhase(devlieryPhaseDto.numberOfHomes, deliveryPhaseGuid);
                //    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                //    return deliveryPhaseToUpdateOrCreate.Id;
               // }
            }
            return Guid.Empty;
        }

        private bool IsNewPhase(DeliveryPhaseDto devlieryPhaseDto, Guid applicationGuid, Guid organisationGuid)
        {
            throw new NotImplementedException();
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
