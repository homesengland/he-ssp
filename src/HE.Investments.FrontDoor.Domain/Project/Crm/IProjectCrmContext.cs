extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

public interface IProjectCrmContext
{
    Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, Guid organisationId, CancellationToken cancellationToken);

    Task<bool> IsThereProjectWithName(string projectName, Guid organisationId, CancellationToken cancellationToken);

    Task<string> Save(FrontDoorProjectDto dto, UserAccount userAccount, CancellationToken cancellationToken);
}
