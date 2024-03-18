using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;

public record GetLocalAuthorityQuery(LocalAuthorityId LocalAuthorityId) : IRequest<Common.Contract.LocalAuthority>;
