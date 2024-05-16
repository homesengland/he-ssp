using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Project.Queries;

public record GetProjectSitesQuery(AhpProjectId ProjectId, PaginationRequest PaginationRequest) : IRequest<ProjectSitesModel>;
