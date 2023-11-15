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

        public HomeTypeDto GetHomeType(string homeTypeId, string fieldsToRetrieve = null)
        {
            HomeTypeDto homeTypeDto = null;
            if(Guid.TryParse(homeTypeId, out var homeTypeGuid))
            {
                var homeType = string.IsNullOrEmpty(fieldsToRetrieve) ?
                    _homeTypeRepository.GetById(homeTypeGuid) :
                    _homeTypeRepository.GetById(homeTypeGuid, fieldsToRetrieve.Split(','));

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
    }
}
