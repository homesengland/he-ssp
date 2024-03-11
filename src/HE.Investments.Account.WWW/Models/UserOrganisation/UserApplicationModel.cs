using HE.Investments.Common.Contract;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record UserApplicationModel(string Id, string Name, ApplicationStatus Status, string Url);
