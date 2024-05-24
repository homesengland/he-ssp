using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Programme.Crm;

public interface IProgrammeCrmContext
{
    Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken);
}
