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
using Microsoft.Xrm.Sdk.Query;

namespace HE.CRM.AHP.Plugins.Services.DeliveryPhase
{
    public class DeliveryPhaseService : CrmService, IDeliveryPhaseService
    {
        private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        public DeliveryPhaseService(CrmServiceArgs args) : base(args)
        {
            _deliveryPhaseRepository = CrmRepositoriesFactory.Get<IDeliveryPhaseRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
        }

        public void DeleteDeliveryPhase(string applicationId, string organisationId, string deliveryPhaseId, string externalUserId)
        {
            if (Guid.TryParse(deliveryPhaseId, out var deliveryPhaseGuid))
            {
                _deliveryPhaseRepository.Delete(new invln_DeliveryPhase()
                {
                    Id = deliveryPhaseGuid
                });
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
                deliveryPhaseDto = DeliveryPhaseMapper.MapRegularEntityToDto(deliveryPhase);
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
                    deliveryPhasesDto.Add(DeliveryPhaseMapper.MapRegularEntityToDto(deliveryPhase));
                }
            }
            return deliveryPhasesDto;
        }

        public Guid SetDeliveryPhase(string deliveryPhase, string userId, string organisationId, string applicationId, string fieldsToSet = null)
        {
            if (Guid.TryParse(applicationId, out var applicationGuid) && Guid.TryParse(organisationId, out var organisationGuid))
            {
                var devlieryPhaseDto = JsonSerializer.Deserialize<DeliveryPhaseDto>(deliveryPhase);
                var deliveryPhaseMapped = DeliveryPhaseMapper.MapDtoToRegularEntity(devlieryPhaseDto);
                var contact = _contactRepository.GetContactViaExternalId(userId);
                if (string.IsNullOrEmpty(devlieryPhaseDto.id) &&
                    _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisation(applicationGuid, organisationGuid))
                {
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return _deliveryPhaseRepository.Create(deliveryPhaseMapped);
                }
                else if (Guid.TryParse(devlieryPhaseDto.id, out var homeTypeGuid))// && _homeTypeRepository.CheckIfGivenHomeTypeIsAssignedToGivenOrganisationAndApplication(homeTypeGuid, organisationGuid, applicationGuid))
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
                    deliveryPhaseToUpdateOrCreate.Id = homeTypeGuid;
                    _deliveryPhaseRepository.Update(deliveryPhaseToUpdateOrCreate);
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return deliveryPhaseToUpdateOrCreate.Id;
                }
            }
            return Guid.Empty;
        }

        private void UpdateApplicationModificationFields(Guid applicationId, Guid contactId)
        {
            var applicationToUpdate = new invln_scheme()
            {
                Id = applicationId,
                invln_lastexternalmodificationon = DateTime.UtcNow,
                invln_lastexternalmodificationby = new Microsoft.Xrm.Sdk.EntityReference(Contact.EntityLogicalName, contactId),
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
