using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public record ApplicationWithFundingDetails(
    ApplicationId ApplicationId,
    string ApplicationName,
    ApplicationStatus Status,
    int? HousesToDeliver,
    decimal? RequiredFunding);
