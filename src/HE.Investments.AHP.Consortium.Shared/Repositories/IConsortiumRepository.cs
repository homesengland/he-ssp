using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract;
using HE.Investments.Programme.Contract;

namespace HE.Investments.AHP.Consortium.Shared.Repositories;

public interface IConsortiumRepository
{
    Task<ConsortiumBasicInfo> GetConsortium(OrganisationId organisationId, ProgrammeId programmeId);
}
