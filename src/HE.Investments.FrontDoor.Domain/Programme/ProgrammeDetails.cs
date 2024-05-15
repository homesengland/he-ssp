namespace HE.Investments.FrontDoor.Domain.Programme;

public record ProgrammeDetails(string Name, DateOnly? StartOn, DateOnly? EndOn, DateOnly? StartOnSiteEndDate);
