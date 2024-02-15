using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetLocalAuthorityQuery(StringIdValueObject LocalAuthorityId) : IRequest<LocalAuthority>;
