using HE.Investments.Common.WWW.Models;

namespace HE.InvestmentLoans.WWW.Models.UserOrganisation;

public record UserOrganisationModel(
    string OrganisationName,
    string UserName,
    bool IsLimitedUser,
    IList<ProgrammeToAccessModel> ProgrammesToToAccess,
    IList<ProgrammeModel> ProgrammesToApply,
    IList<ActionModel> Actions);
