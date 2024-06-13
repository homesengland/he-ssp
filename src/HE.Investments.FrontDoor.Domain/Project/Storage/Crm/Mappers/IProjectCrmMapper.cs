using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Crm.Mappers;

public interface IProjectCrmMapper
{
    FrontDoorProjectDto ToDto(ProjectEntity entity, UserAccount userAccount);

    ProjectEntity ToEntity(FrontDoorProjectDto dto);
}
