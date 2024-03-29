using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Common.User;
using MediatR;

namespace HE.Investments.Account.Domain.User.QueryHandlers;

public class GetUserProfileInformationQueryHandler : IRequestHandler<GetUserProfileDetailsQuery, UserProfileDetails>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IUserContext _userContext;

    public GetUserProfileInformationQueryHandler(IProfileRepository profileRepository, IUserContext userContext)
    {
        _profileRepository = profileRepository;
        _userContext = userContext;
    }

    public async Task<UserProfileDetails> Handle(GetUserProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        var userGlobalId = request.UserGlobalId ?? UserGlobalId.From(_userContext.UserGlobalId);
        var userProfileInformation = await _profileRepository.GetProfileDetails(userGlobalId);

        return new UserProfileDetails
        {
            FirstName = userProfileInformation.FirstName?.ToString(),
            LastName = userProfileInformation.LastName?.ToString(),
            JobTitle = userProfileInformation.JobTitle?.ToString(),
            Email = userProfileInformation.Email,
            TelephoneNumber = userProfileInformation.TelephoneNumber?.ToString(),
            SecondaryTelephoneNumber = userProfileInformation.SecondaryTelephoneNumber?.ToString(),
            IsTermsAndConditionsAccepted = userProfileInformation.IsTermsAndConditionsAccepted,
        };
    }
}
