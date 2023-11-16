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
        public HomeTypeService(CrmServiceArgs args) : base(args)
        {
            _homeTypeRepository = CrmRepositoriesFactory.Get<IHomeTypeRepository>();
        }

        public void DeleteHomeType(string homeTypeId, string userId)
        {
            if (Guid.TryParse(homeTypeId, out var homeTypeGuid))
            {
                if (_homeTypeRepository.CheckIfGivenHomeTypeIsAssignedToGivenUser(homeTypeGuid, userId))
                {
                    _homeTypeRepository.Delete(new invln_HomeType() { Id = homeTypeGuid });
                }
            }
        }

        public List<HomeTypeDto> GetApplicaitonHomeTypes(string applicationId)
        {
            var listOfHomeTypesDto = new List<HomeTypeDto>();
            if (Guid.TryParse(applicationId, out var applicationGuid))
            {
                var homeTypes = _homeTypeRepository.GetHomeTypesRelatedToApplication(applicationGuid);
                if (homeTypes.Any())
                {
                    foreach (var homeType in homeTypes)
                    {
                        listOfHomeTypesDto.Add(HomeTypeMapper.MapRegularEntityToDto(homeType));
                    }
                }
            }
            return listOfHomeTypesDto;
        }

        public HomeTypeDto GetHomeType(string homeTypeId, string applicationId, string fieldsToRetrieve = null)
        {
            HomeTypeDto homeTypeDto = null;
            string attributes = null;
            if (!string.IsNullOrEmpty(fieldsToRetrieve))
            {
                attributes = GenerateFetchXmlAttributes(fieldsToRetrieve);
            }
            var homeType = _homeTypeRepository.GetHomeTypeByIdAndApplicationId(homeTypeId, applicationId, attributes);
            if (homeType != null)
            {
                homeTypeDto = HomeTypeMapper.MapRegularEntityToDto(homeType);
            }
            return homeTypeDto;
        }

        public void SetHomeType(string homeType, string fieldsToSet = null)
        {
            var homeTypeDto = JsonSerializer.Deserialize<HomeTypeDto>(homeType);
            var homeTypeMapped = HomeTypeMapper.MapDtoToRegularEntity(homeTypeDto);
            invln_HomeType homeTypeToUpdateOrCreate;
            if (!string.IsNullOrEmpty(fieldsToSet))
            {
                var fields = fieldsToSet.Split(',');
                homeTypeToUpdateOrCreate = new invln_HomeType();
                foreach (var field in fields)
                {
                    TracingService.Trace($"field name {field}");
                    homeTypeToUpdateOrCreate[field] = homeTypeMapped[field];
                }
            }
            else
            {
                homeTypeToUpdateOrCreate = homeTypeMapped;
            }

            if (string.IsNullOrEmpty(homeTypeDto.id))
            {
                _ = _homeTypeRepository.Create(homeTypeToUpdateOrCreate);
            }
            else
            {
                homeTypeToUpdateOrCreate.Id = new Guid(homeTypeDto.id);
                _homeTypeRepository.Update(homeTypeToUpdateOrCreate);
            }
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
