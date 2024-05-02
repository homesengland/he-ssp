using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.AHP.Consortium.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Repositories;

public interface IIsPartOfConsortium
{
    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default);
}
