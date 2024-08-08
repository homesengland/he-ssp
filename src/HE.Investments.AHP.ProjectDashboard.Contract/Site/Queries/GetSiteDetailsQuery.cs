using HE.Investment.AHP.Contract.Site;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Site.Queries;

public record GetSiteDetailsQuery(SiteId SiteId) : IRequest<SiteDetailsModel>;
