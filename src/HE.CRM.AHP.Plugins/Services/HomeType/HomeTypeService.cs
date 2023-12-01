using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;
using Microsoft.Crm.Sdk.Messages;

namespace HE.CRM.AHP.Plugins.Services.HomeType
{
    public class HomeTypeService : CrmService, IHomeTypeService
    {
        private readonly IHomeTypeRepository _homeTypeRepository;
        private readonly IAhpApplicationRepository _ahpApplicationRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ISharepointDocumentLocationRepository _sharepointDocumentLocationRepository;
        public HomeTypeService(CrmServiceArgs args) : base(args)
        {
            _homeTypeRepository = CrmRepositoriesFactory.Get<IHomeTypeRepository>();
            _ahpApplicationRepository = CrmRepositoriesFactory.Get<IAhpApplicationRepository>();
            _contactRepository = CrmRepositoriesFactory.Get<IContactRepository>();
            _sharepointDocumentLocationRepository = CrmRepositoriesFactory.Get<ISharepointDocumentLocationRepository>();
        }

        public void DeleteHomeType(string homeTypeId, string userId, string organisationId, string applicationId)
        {
            if (Guid.TryParse(homeTypeId, out var homeTypeGuid) && Guid.TryParse(organisationId, out var organisationGuid) &&
                Guid.TryParse(applicationId, out var applicationGuid))
            {
                if (_homeTypeRepository.CheckIfGivenHomeTypeIsAssignedToGivenUserAndOrganisationAndApplication(homeTypeGuid, userId, organisationGuid, applicationGuid))
                {
                    var contact = _contactRepository.GetContactViaExternalId(userId);
                    _homeTypeRepository.Delete(new invln_HomeType() { Id = homeTypeGuid });
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                }
            }
        }

        public List<HomeTypeDto> GetApplicaitonHomeTypes(string applicationId, string userId, string organisationId, string fieldsToRetrieve = null)
        {
            var listOfHomeTypesDto = new List<HomeTypeDto>();
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var homeTypes = _homeTypeRepository.GetHomeTypesForUserAndOrganisationRelatedToApplication(applicationId, userId, organisationId, attributes);
            if (homeTypes.Any())
            {
                foreach (var homeType in homeTypes)
                {
                    listOfHomeTypesDto.Add(HomeTypeMapper.MapRegularEntityToDto(homeType));
                }
            }
            return listOfHomeTypesDto;
        }

        public HomeTypeDto GetHomeType(string homeTypeId, string applicationId, string userId, string organisationId, string fieldsToRetrieve = null)
        {
            HomeTypeDto homeTypeDto = null;
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var homeType = _homeTypeRepository.GetHomeTypeForUserAndOrganisationByIdAndApplicationId(homeTypeId, applicationId, userId, organisationId, attributes);
            if (homeType != null)
            {
                homeTypeDto = HomeTypeMapper.MapRegularEntityToDto(homeType);
            }
            return homeTypeDto;
        }

        public Guid SetHomeType(string homeType, string userId, string organisationId, string applicationId, string fieldsToSet = null)
        {
            if (Guid.TryParse(applicationId, out var applicationGuid) && Guid.TryParse(organisationId, out var organisationGuid))
            {
                var homeTypeDto = JsonSerializer.Deserialize<HomeTypeDto>(homeType);
                var homeTypeMapped = HomeTypeMapper.MapDtoToRegularEntity(homeTypeDto, applicationId);
                var contact = _contactRepository.GetContactViaExternalId(userId);
                if (string.IsNullOrEmpty(homeTypeDto.id) &&
                    _ahpApplicationRepository.ApplicationWithGivenIdExistsForOrganisationAndContract(applicationGuid, organisationGuid, userId))
                {
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return _homeTypeRepository.Create(homeTypeMapped);
                }
                else if (Guid.TryParse(homeTypeDto.id, out var homeTypeGuid) &&
                    _homeTypeRepository.CheckIfGivenHomeTypeIsAssignedToGivenUserAndOrganisationAndApplication(homeTypeGuid, userId, organisationGuid, applicationGuid))
                {
                    invln_HomeType homeTypeToUpdateOrCreate;
                    if (!string.IsNullOrEmpty(fieldsToSet))
                    {
                        var fields = fieldsToSet.Split(',');
                        homeTypeToUpdateOrCreate = new invln_HomeType();
                        foreach (var field in fields)
                        {
                            TracingService.Trace($"field name {field}");
                            if (homeTypeMapped.Contains(field))
                            {
                                TracingService.Trace($"contains");
                                homeTypeToUpdateOrCreate[field] = homeTypeMapped[field];
                            }
                        }
                    }
                    else
                    {
                        homeTypeToUpdateOrCreate = homeTypeMapped;
                    }
                    homeTypeToUpdateOrCreate.Id = homeTypeGuid;
                    _homeTypeRepository.Update(homeTypeToUpdateOrCreate);
                    UpdateApplicationModificationFields(applicationGuid, contact.Id);
                    return homeTypeToUpdateOrCreate.Id;
                }
            }
            return Guid.Empty;
        }

        public void SetHappiPrinciplesValue(invln_HomeType target)
        {
            if (target.invln_happiprinciples != null && target.invln_happiprinciples.Any(x => x.Value == (int)invln_HAPPIprinciples.KNone))
            {
                target.invln_happiprinciples.Clear();
                target.invln_happiprinciples.Add(new Microsoft.Xrm.Sdk.OptionSetValue((int)invln_HAPPIprinciples.KNone));
            }
        }

        public void CreateDocumentLocation(invln_HomeType target)
        {
            if (target.invln_application != null)
            {
                var applicationDocumentLocation = _sharepointDocumentLocationRepository.GetDocumentLocationRelatedToRecordWithGivenGuid(target.invln_application.Id);
                var homeTypeLocation = _sharepointDocumentLocationRepository.GetHomeTypeDocumentLocationForGivenApplicationLocationRecord(applicationDocumentLocation.Id);
                var locationToCreate = new SharePointDocumentLocation()
                {
                    RegardingObjectId = target.ToEntityReference(),
                    Name = $"Home Type For Ahp Application",
                    RelativeUrl = $"{target.invln_hometypename}-{target.Id}",
                    ParentSiteOrLocation = homeTypeLocation.ToEntityReference(),
                };
                _ = _sharepointDocumentLocationRepository.Create(locationToCreate);
            }
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
