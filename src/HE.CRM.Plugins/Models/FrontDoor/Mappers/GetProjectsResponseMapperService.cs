using System.Collections.Generic;
using System.Linq;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Repositories.Interfaces;
using HE.CRM.Plugins.Models.FrontDoor.Contract.Responses;

namespace HE.CRM.Plugins.Models.Frontdoor.Mappers
{
    public class GetProjectsResponseMapperService : CrmService, IGetProjectsResponseMapperService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IGetProjectResponseMapperService _getProjectResponseMapperService;

        public GetProjectsResponseMapperService(CrmServiceArgs args,
            IContactRepository contactRepository,
            IGetProjectResponseMapperService getProjectResponseMapperService)
            : base(args)
        {
            _getProjectResponseMapperService = getProjectResponseMapperService;
            _contactRepository = contactRepository;
        }

        public IEnumerable<FrontDoorProjectDto> Map(GetProjectsResponse response)
        {
            var portalOwnerIdList = response.Select(x => x.PortalOwnerId);
            var contactsExternalIdMap = _contactRepository.GetContactsMapExternalIds(portalOwnerIdList);

            return response.Select(x => _getProjectResponseMapperService.Map(x, contactsExternalIdMap));
        }
    }
}
