namespace HE.Investments.FrontDoor.Contract.Project;

public record ProjectDetails(
    FrontDoorProjectId ProjectId,
    string Name,
    bool IsEnglandHousingDelivery);
