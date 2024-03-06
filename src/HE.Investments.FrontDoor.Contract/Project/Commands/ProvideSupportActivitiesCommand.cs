using HE.Investments.FrontDoor.Contract.Project.Enums;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideSupportActivitiesCommand(FrontDoorProjectId ProjectId, IList<SupportActivityType> ActivityTypes) : IProvideProjectDetailsCommand;
