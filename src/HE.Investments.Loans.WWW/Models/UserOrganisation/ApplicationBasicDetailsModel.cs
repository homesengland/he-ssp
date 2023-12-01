using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.WWW.Models.UserOrganisation;

public record ApplicationBasicDetailsModel(Guid Id, string ApplicationName, ApplicationStatus Status);
