extern alias Org;

using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;

public class ProvideUserDetailsCommandHandler : IRequestHandler<ProvideUserDetailsCommand, OperationResult>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly ILoanUserRepository _loanUserRepository;
    private readonly ILogger<ProvideUserDetailsCommandHandler> _logger;

    public ProvideUserDetailsCommandHandler(ILoanUserContext loanUserContext, ILoanUserRepository loanUserRepository, ILogger<ProvideUserDetailsCommandHandler> logger)
    {
        _loanUserContext = loanUserContext;
        _loanUserRepository = loanUserRepository;
        _logger = logger;
    }

    public async Task<OperationResult> Handle(ProvideUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var userDetails = await _loanUserRepository.GetUserDetails(_loanUserContext.UserGlobalId);

        try
        {
            userDetails.ProvideUserDetails(
                request.FirstName,
                request.LastName,
                request.JobTitle,
                request.TelephoneNumber,
                request.SecondaryTelephoneNumber,
                _loanUserContext.Email);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _loanUserRepository.SaveAsync(userDetails, _loanUserContext.UserGlobalId, cancellationToken);
        await _loanUserContext.RefreshUserData();

        return OperationResult.Success();
    }
}
