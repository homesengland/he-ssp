using HE.Investments.Common.Contract;
using MediatR;

namespace HE.Investments.Account.Contract.User.Queries;

public record GetUserProfileDetailsQuery(UserGlobalId? UserGlobalId = null) : IRequest<UserProfileDetailsModel>;
