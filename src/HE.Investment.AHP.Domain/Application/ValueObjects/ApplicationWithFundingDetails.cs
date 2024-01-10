using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public record ApplicationWithFundingDetails(
    AhpApplicationId ApplicationId,
    string ApplicationName,
    ApplicationStatus Status,
    Tenure Tenure,
    int? HousesToDeliver,
    decimal? RequiredFunding);
