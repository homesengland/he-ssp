using MediatR;

namespace HE.Investment.AHP.Contract.Project.Queries;

public record GetProjectDetailsQuery(AhpProjectId ProjectId) : IRequest<ProjectDetailsModel>;
