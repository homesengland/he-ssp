using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.Crm;

public interface IApplicationCrmContext
{
    Task<AhpApplicationDto> GetOrganisationApplicationById(string id, Guid organisationId, CancellationToken cancellationToken);

    Task<AhpApplicationDto> GetUserApplicationById(string id, Guid organisationId, CancellationToken cancellationToken);

    Task<bool> IsNameExist(string applicationName, Guid organisationId, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetOrganisationApplications(Guid organisationId, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetUserApplications(Guid organisationId, CancellationToken cancellationToken);

    Task<string> Save(AhpApplicationDto dto, Guid organisationId, CancellationToken cancellationToken);

    Task ChangeApplicationStatus(
        string applicationId,
        Guid organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        bool representationsAndWarranties,
        CancellationToken cancellationToken);
}
