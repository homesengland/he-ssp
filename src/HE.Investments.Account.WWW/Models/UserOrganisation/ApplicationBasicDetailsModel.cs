using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ApplicationBasicDetailsModel(string Id, string ApplicationName, ApplicationStatus Status, string ApplicationUrl);
