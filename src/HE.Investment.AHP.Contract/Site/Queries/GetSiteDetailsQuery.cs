using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSiteDetailsQuery(string SiteId) : IRequest<SiteDetailsModel>;
