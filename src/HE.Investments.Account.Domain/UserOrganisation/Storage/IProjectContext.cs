using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage;

public interface IProjectContext
{
    Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string organisationId, CancellationToken cancellationToken);

    Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken);
}
