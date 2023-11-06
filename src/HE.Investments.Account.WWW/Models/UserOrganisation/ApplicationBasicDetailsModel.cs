using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.Investments.Account.WWW.Models.UserOrganisation;

public record ApplicationBasicDetailsModel(Guid Id, string ApplicationName, ApplicationStatus Status);
