using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Data;

public interface IApplicationCrmContext
{
    Task<AhpApplicationDto> GetOrganisationApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<AhpApplicationDto> GetUserApplicationById(string id, Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<bool> IsNameExist(string applicationName, Guid organisationId, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetOrganisationApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetUserApplications(Guid organisationId, IList<string> fieldsToRetrieve, CancellationToken cancellationToken);

    Task<string> Save(AhpApplicationDto dto, Guid organisationId, IList<string> fieldsToUpdate, CancellationToken cancellationToken);

    Task ChangeApplicationStatus(string applicationId, Guid organisationId, ApplicationStatus applicationStatus, string? reason,
        CancellationToken cancellationToken);
}
