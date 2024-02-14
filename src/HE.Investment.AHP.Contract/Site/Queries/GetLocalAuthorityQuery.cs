using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetLocalAuthorityQuery(string LocalAuthorityId) : IRequest<LocalAuthority>;
