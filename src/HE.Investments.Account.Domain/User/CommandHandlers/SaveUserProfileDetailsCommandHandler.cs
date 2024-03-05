using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Domain.User.CommandHandlers;

public class SaveUserProfileDetailsCommandHandler : IRequestHandler<SaveUserProfileDetailsCommand, OperationResult>
{
    private readonly IProfileRepository _profileRepository;

    private readonly IAccountUserContext _accountContext;

    public SaveUserProfileDetailsCommandHandler(IProfileRepository profileRepository, IAccountUserContext accountContext)
    {
        _profileRepository = profileRepository;
        _accountContext = accountContext;
    }

    public async Task<OperationResult> Handle(SaveUserProfileDetailsCommand request, CancellationToken cancellationToken)
    {
        var userDetails = await _profileRepository.GetProfileDetails(_accountContext.UserGlobalId);

        userDetails.ProvideUserProfileDetails(
            request.FirstName,
            request.LastName,
            request.JobTitle,
            request.TelephoneNumber,
            request.SecondaryTelephoneNumber,
            _accountContext.Email);

        await _profileRepository.SaveAsync(userDetails, _accountContext.UserGlobalId, cancellationToken);

        return OperationResult.Success();
    }
}
