using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;

namespace HE.Investments.Account.WWW.Utils;

public interface IProgrammes
{
    ProgrammeModel GetProgramme(ProgrammeType programmeType);

    string GetUrl(ProgrammeType programmeType, HeApplianceId id);
}
