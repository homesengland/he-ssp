using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Programme;

public record AhpProgramme(string Name, ProgrammeDates ProgrammeDates, MilestoneFramework MilestoneFramework);
