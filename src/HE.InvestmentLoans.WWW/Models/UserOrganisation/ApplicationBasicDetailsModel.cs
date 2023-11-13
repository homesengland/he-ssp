using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.WWW.Models.UserOrganisation;

public record ApplicationBasicDetailsModel(Guid Id, string ApplicationName, ApplicationStatus Status);
