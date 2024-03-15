using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideAffordableHomesAmountCommand(FrontDoorProjectId ProjectId, AffordableHomesAmount AffordableHomesAmount)
    : IProvideProjectDetailsCommand;
