extern alias Org;

using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Consortium.Domain.Crm;

public interface IConsortiumCrmContext
{
    Task<ConsortiumDto> GetConsortium(string consortiumId, string organisationId, CancellationToken cancellationToken);

    Task<string> CreateConsortium(
        string userId,
        string programmeId,
        string consortiumName,
        string leadOrganisationId,
        CancellationToken cancellationToken);

    Task<bool> IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId, CancellationToken cancellationToken);
}
