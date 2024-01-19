using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Services;

public interface IProgrammeService
{
    Task<ProgrammeDto?> Get(CancellationToken cancellationToken);
}
