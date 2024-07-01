using System.Collections.Generic;
using System.Linq;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;
using HE.CRM.Common.Repositories.Interfaces;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
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
            // get all contacts ids to create a map ContactId, ContactExternalId
            var contactIdList = response.Select(x => x.PortalOwnerId);
            contactIdList = response.Select(x => x.FrontDoorProjectContact.ContactId);
            var contactsExternalIdMap = _contactRepository.GetContactsMapExternalIds(contactIdList.Distinct());

            return response.Select(x => _getProjectResponseMapperService.Map(x, contactsExternalIdMap));
        }
    }
}
