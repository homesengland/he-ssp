using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideExpectedStartDateCommand(FrontDoorProjectId ProjectId, DateDetails? ExpectedStartDate) : IProvideProjectDetailsCommand;
