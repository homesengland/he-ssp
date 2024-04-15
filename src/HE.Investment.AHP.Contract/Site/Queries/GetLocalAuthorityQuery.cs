using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;
using LocalAuthority = HE.Investments.Common.Contract.LocalAuthority;

namespace HE.Investment.AHP.Contract.Site.Queries;

public record GetLocalAuthorityQuery(LocalAuthorityCode LocalAuthorityCode) : IRequest<LocalAuthority>;
