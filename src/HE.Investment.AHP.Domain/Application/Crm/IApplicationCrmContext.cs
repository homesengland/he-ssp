using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.Crm;

public interface IApplicationCrmContext
{
    Task<AhpApplicationDto> GetOrganisationApplicationById(string id, string organisationId, CancellationToken cancellationToken);

    Task<AhpApplicationDto> GetUserApplicationById(string id, string organisationId, CancellationToken cancellationToken);

    Task<bool> IsNameExist(string applicationName, string organisationId, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetOrganisationApplications(string organisationId, CancellationToken cancellationToken);

    Task<IList<AhpApplicationDto>> GetUserApplications(string organisationId, CancellationToken cancellationToken);

    Task<string> Save(AhpApplicationDto dto, string organisationId, CancellationToken cancellationToken);

    Task ChangeApplicationStatus(
        string applicationId,
        string organisationId,
        ApplicationStatus applicationStatus,
        string? changeReason,
        bool representationsAndWarranties,
        CancellationToken cancellationToken);
}
