using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSiteListQuery(PaginationRequest PaginationRequest) : IRequest<SitesListModel>;
