using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;

public class AhpProjectName(string? value) : YourShortText(value, "Name", "project name");
