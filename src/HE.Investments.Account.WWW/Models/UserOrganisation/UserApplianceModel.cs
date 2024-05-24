using HE.Investments.Common.Contract;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record UserApplianceModel(string Id, string Name, ApplicationStatus? Status, string Url);
