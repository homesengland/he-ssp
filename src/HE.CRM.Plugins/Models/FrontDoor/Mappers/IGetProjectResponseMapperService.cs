using System;
using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Plugins.Models.FrontDoor.Contract.Responses;

namespace HE.CRM.Plugins.Models.Frontdoor.Mappers
{
    public interface IGetProjectResponseMapperService : ICrmService
    {
        FrontDoorProjectDto Map(GetProjectResponse response, Dictionary<Guid, string> contactsExternalIdMap);
    }
}
