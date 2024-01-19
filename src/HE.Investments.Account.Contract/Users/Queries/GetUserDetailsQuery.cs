using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investments.Account.Contract.Users.Queries;

public record GetUserDetailsQuery(UserGlobalId Id) : IRequest<(string OrganisationName, UserDetails UserDetails)>;
