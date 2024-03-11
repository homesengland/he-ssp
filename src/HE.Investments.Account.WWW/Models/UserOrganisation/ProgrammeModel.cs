using HE.Investments.Account.Contract.UserOrganisation;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ProgrammeModel(
    ProgrammeType Type,
    string Name,
    string Description,
    string ViewAllApplicationsUrl);
