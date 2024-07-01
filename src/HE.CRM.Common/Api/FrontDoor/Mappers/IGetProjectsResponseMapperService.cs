using System.Collections.Generic;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public interface IGetProjectsResponseMapperService : ICrmService
    {
        IEnumerable<FrontDoorProjectDto> Map(GetProjectsResponse response);
    }
}
