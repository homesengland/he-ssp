extern alias Org;

using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.FrontDoor.Domain.Programme.Crm;

public interface IProgrammeCrmContext
{
    Task<IList<ProgrammeDto>> GetProgrammes(CancellationToken cancellationToken);
}
