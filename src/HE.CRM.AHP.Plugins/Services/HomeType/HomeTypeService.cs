using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.DtoMapping;
using HE.CRM.Common.Repositories.Interfaces;

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
    }
}
