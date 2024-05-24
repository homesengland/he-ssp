using HE.Investments.Common.Contract;
using HE.Investments.Programme.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IIsPartOfConsortium
{
    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default);
}
