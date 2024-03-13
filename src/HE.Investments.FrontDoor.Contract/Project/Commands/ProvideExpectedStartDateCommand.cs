using HE.Investments.Common.Contract;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideExpectedStartDateCommand(FrontDoorProjectId ProjectId, DateDetails? ExpectedStartDate) : IProvideProjectDetailsCommand;
