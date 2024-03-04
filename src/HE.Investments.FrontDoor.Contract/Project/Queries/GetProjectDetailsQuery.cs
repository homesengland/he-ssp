using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Queries;

public record GetProjectDetailsQuery(FrontDoorProjectId ProjectId) : IRequest<ProjectDetails>;
