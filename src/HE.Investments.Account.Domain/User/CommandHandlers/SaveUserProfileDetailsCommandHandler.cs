using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Account.Domain.User.Commands;
using HE.Investments.Account.Domain.User.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Account.Domain.User.CommandHandlers;

public class SaveUserProfileDetailsCommandHandler : IRequestHandler<SaveUserProfileDetailsCommand, OperationResult>
{
    private readonly IUserRepository _userRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ILogger<SaveUserProfileDetailsCommandHandler> _logger;

    public SaveUserProfileDetailsCommandHandler(IUserRepository userRepository, IAccountUserContext accountUserContext, ILogger<SaveUserProfileDetailsCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(SaveUserProfileDetailsCommand request, CancellationToken cancellationToken)
    {
        var userDetails = await _userRepository.GetUserProfileInformation(_accountUserContext.UserGlobalId);

        try
        {
            userDetails.ProvideUserProfileDetails(
                request.FirstName,
                request.LastName,
                request.JobTitle,
                request.TelephoneNumber,
                request.SecondaryTelephoneNumber,
                _accountUserContext.Email);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _userRepository.SaveAsync(userDetails, _accountUserContext.UserGlobalId, cancellationToken);
        _accountUserContext.RefreshUserData();

        return OperationResult.Success();
    }
}
