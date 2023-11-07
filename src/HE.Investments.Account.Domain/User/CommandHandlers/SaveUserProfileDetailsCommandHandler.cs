using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.Account.Domain.User.Commands;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.User.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Account.Domain.User.CommandHandlers;

public class SaveUserProfileDetailsCommandHandler : IRequestHandler<SaveUserProfileDetailsCommand, OperationResult>
{
    private readonly IUserRepository _userRepository;

    private readonly IUserContext _userContext;

    private readonly ILogger<SaveUserProfileDetailsCommandHandler> _logger;

    public SaveUserProfileDetailsCommandHandler(IUserRepository userRepository, IUserContext userContext, ILogger<SaveUserProfileDetailsCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<OperationResult> Handle(SaveUserProfileDetailsCommand request, CancellationToken cancellationToken)
    {
        var userGlobalId = UserGlobalId.From(_userContext.UserGlobalId);
        var userDetails = await _userRepository.GetUserProfileInformation(userGlobalId);

        try
        {
            userDetails.ProvideUserProfileDetails(
                request.FirstName,
                request.LastName,
                request.JobTitle,
                request.TelephoneNumber,
                request.SecondaryTelephoneNumber,
                _userContext.Email);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _userRepository.SaveAsync(userDetails, userGlobalId, cancellationToken);

        return OperationResult.Success();
    }
}
