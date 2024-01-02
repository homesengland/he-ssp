using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetSiteListQuery : IRequest<SitesListModel>;
