using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSiteBasicDetailsQuery(string SiteId) : IRequest<SiteBasicModel>;
