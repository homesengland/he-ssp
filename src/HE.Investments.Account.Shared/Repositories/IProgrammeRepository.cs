using HE.Investments.Account.Contract.UserOrganisation;

namespace HE.Investments.Account.Shared.Repositories;

public interface IProgrammeRepository
{
    Task<ProgrammeBasicInfo> GetCurrentProgramme(ProgrammeType programmeType);
}
