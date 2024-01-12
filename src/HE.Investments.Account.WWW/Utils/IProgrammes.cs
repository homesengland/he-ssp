using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;

namespace HE.Investments.Account.WWW.Utils;

public interface IProgrammes
{
    Task<ProgrammeModel> GetProgramme(ProgrammeType programmeType);

    string GetApplicationUrl(ProgrammeType programmeType, HeApplicationId applicationId);
}
