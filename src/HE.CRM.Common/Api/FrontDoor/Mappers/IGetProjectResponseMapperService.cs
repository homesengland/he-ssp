using System;
using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public interface IGetProjectResponseMapperService : ICrmService
    {
        FrontDoorProjectDto Map(GetProjectResponse response, Dictionary<Guid, string> contactsExternalIdMap);
    }
}
