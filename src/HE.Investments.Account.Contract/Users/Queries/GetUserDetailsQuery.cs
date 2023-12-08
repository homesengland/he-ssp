using MediatR;

namespace HE.Investments.Account.Contract.Users.Queries;

public record GetUserDetailsQuery(string Id) : IRequest<(string OrganisationName, UserDetails UserDetails)>;
