using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Programme.Domain.Crm;

public interface IProgrammeCrmContext
{
    Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken);

    Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken);
}
