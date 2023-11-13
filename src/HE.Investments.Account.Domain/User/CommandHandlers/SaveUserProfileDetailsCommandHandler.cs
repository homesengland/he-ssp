using HE.Investments.Account.Domain.User.Commands;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.User;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Account.Domain.User.CommandHandlers;

public class SaveUserProfileDetailsCommandHandler : IRequestHandler<SaveUserProfileDetailsCommand, OperationResult>
{
    private readonly IUserRepository _userRepository;

    private readonly IAccountUserContext _accountContext;

    private readonly ILogger<SaveUserProfileDetailsCommandHandler> _logger;

    public SaveUserProfileDetailsCommandHandler(IUserRepository userRepository, IAccountUserContext accountContext, ILogger<SaveUserProfileDetailsCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
        _accountContext = accountContext;
    }

    public async Task<OperationResult> Handle(SaveUserProfileDetailsCommand request, CancellationToken cancellationToken)
    {
        var userDetails = await _userRepository.GetProfileDetails(_accountContext.UserGlobalId);

        try
        {
            userDetails.ProvideUserProfileDetails(
                request.FirstName,
                request.LastName,
                request.JobTitle,
                request.TelephoneNumber,
                request.SecondaryTelephoneNumber,
                _accountContext.Email);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _userRepository.SaveAsync(userDetails, _accountContext.UserGlobalId, cancellationToken);
        await _accountContext.RefreshProfileDetails();

        return OperationResult.Success();
    }
}
