using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Queries;

public record ValidateProjectAnswersQuery(FrontDoorProjectId ProjectId) : IRequest<ApplicationType>;
