using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Project;

public record ApplicationProjectModel(AhpApplicationId Id, string Name, ApplicationStatus Status, decimal? Grant, int? Unit);
