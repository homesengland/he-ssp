using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;

namespace HE.Investments.Consortium.Shared.Repositories;

public interface IConsortiumRepository
{
    Task<ConsortiumBasicInfo> GetConsortium(OrganisationId organisationId, ProgrammeId programmeId);
}
