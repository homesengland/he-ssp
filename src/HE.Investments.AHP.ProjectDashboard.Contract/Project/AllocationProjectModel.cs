using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project;

public record AllocationProjectModel(string Id, string Name, Tenure Tenure, int HouseToDeliver, string LocalAuthorityName, bool? HasMilestoneInDueState = null);
