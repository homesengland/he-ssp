using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record UserOrganisationModel(
    string OrganisationName,
    string? UserName,
    bool IsLimitedUser,
    string? StartFrontDoorProjectUrl,
    IList<UserProjectModel> Projects,
    IList<ProgrammeToAccessModel> ProgrammesToAccess,
    IList<ActionModel> Actions);
