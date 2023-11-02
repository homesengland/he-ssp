using MediatR;

namespace HE.Investments.Account.Contract.User.Queries;

public record GetUserProfileDetailsQuery : IRequest<UserProfileDetailsModel>;
