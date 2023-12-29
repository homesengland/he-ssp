using HE.Investments.Common.Contract;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ApplicationBasicDetailsModel(string Id, string ApplicationName, ApplicationStatus Status, string ApplicationUrl);
