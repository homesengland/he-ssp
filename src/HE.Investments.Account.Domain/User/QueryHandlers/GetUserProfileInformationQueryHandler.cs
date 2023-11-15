using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.User;
using MediatR;

namespace HE.Investments.Account.Domain.User.QueryHandlers;

public class GetUserProfileInformationQueryHandler : IRequestHandler<GetUserProfileDetailsQuery, UserProfileDetailsModel>
{
    private readonly IUserRepository _userRepository;

    private readonly IUserContext _userContext;

    public GetUserProfileInformationQueryHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<UserProfileDetailsModel> Handle(GetUserProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        var userProfileInformation = await _userRepository.GetProfileDetails(UserGlobalId.From(_userContext.UserGlobalId));

        return new UserProfileDetailsModel
        {
            FirstName = userProfileInformation.FirstName?.ToString(),
            LastName = userProfileInformation.LastName?.ToString(),
            JobTitle = userProfileInformation.JobTitle?.ToString(),
            Email = userProfileInformation.Email,
            TelephoneNumber = userProfileInformation.TelephoneNumber?.ToString(),
            SecondaryTelephoneNumber = userProfileInformation.SecondaryTelephoneNumber?.ToString(),
        };
    }
}
