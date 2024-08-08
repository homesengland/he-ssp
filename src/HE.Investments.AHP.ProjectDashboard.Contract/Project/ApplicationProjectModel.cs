using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record ApplicationProjectModel(AhpApplicationId Id, string Name, ApplicationStatus Status, decimal? Grant, int? Unit, string? LocalAuthorityName);
