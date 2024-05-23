using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectName(string? value) : YourShortText(value, "Name", "project name");
