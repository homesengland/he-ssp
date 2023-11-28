using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.WWW.Models.UserOrganisation;

namespace HE.Investments.Account.WWW.Routing;

public interface IProgrammes
{
    ProgrammeModel GetProgramme(ProgrammeType programmeType);

    string GetApplicationUrl(ProgrammeType programmeType, string applicationId);
}
