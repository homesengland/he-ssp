using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Queries;

public record GetLocalAuthorityQuery(LocalAuthorityId LocalAuthorityId) : IRequest<LocalAuthority>;
