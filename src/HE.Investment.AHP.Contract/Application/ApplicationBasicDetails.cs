using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Application;

public record ApplicationBasicDetails(
    AhpApplicationId Id,
    string Name,
    ApplicationStatus Status,
    Tenure? Tenure,
    decimal? Grant,
    int? Unit);
