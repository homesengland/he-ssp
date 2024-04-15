using HE.Investment.AHP.Contract.Site;
using MediatR;

namespace HE.Investment.AHP.Contract.PrefillData.Queries;

public record GetAhpSitePrefillDataQuery(SiteId SiteId) : IRequest<AhpSitePrefillData>;
