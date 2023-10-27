using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using MediatR;

namespace HE.Investments.Account.Domain.User.QueryHandlers;

public class GetUserProfileDetailsQueryHandler : IRequestHandler<GetUserProfileDetailsQuery, UserProfileDetailsViewModel>
{
    public Task<UserProfileDetailsViewModel> Handle(GetUserProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new UserProfileDetailsViewModel
        {
            FirstName = "John",
            LastName = "Rambo",
            JobTitle = "assassin",
            TelephoneNumber = "+1 111 222 333",
            SecondaryTelephoneNumber = null,
        });
    }
}
