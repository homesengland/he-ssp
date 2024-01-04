using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSiteQuery(string SiteId) : IRequest<SiteModel>;
