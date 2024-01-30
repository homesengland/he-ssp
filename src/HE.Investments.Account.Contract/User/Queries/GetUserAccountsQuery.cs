using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investments.Account.Contract.User.Queries;

public record GetUserAccountsQuery(UserGlobalId UserGlobalId, string UserEmailAddress) : IRequest<AccountDetails>;
