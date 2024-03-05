using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideAffordableHomesAmountCommand(FrontDoorProjectId ProjectId, AffordableHomesAmount AffordableHomesAmount)
    : IProvideProjectDetailsCommand;
