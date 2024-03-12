using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideRequiredFundingCommand(FrontDoorProjectId ProjectId, RequiredFundingOption? RequiredFundingOption) : IProvideProjectDetailsCommand;
