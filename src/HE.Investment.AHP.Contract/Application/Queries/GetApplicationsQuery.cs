using HE.Investments.Common.Utils.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationsQuery(PaginationRequest PaginationRequest) : IRequest<GetApplicationsQueryResult>;
