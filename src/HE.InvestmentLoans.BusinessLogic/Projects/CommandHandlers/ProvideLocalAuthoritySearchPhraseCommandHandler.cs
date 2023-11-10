using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLocalAuthoritySearchPhraseCommandHandler : IRequestHandler<ProvideLocalAuthoritySearchPhraseCommand, OperationResult>
{
    private readonly ILogger<ProjectCommandHandlerBase> _logger;

    public ProvideLocalAuthoritySearchPhraseCommandHandler(ILogger<ProjectCommandHandlerBase> logger)
    {
        _logger = logger;
    }

    public Task<OperationResult> Handle(ProvideLocalAuthoritySearchPhraseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Phrase.IsNotProvided())
            {
                OperationResult
                    .New()
                    .AddValidationError(nameof(LocalAuthoritiesViewModel.Phrase), ValidationErrorMessage.LocalAuthorityNameIsEmpty)
                    .CheckErrors();
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);

            return Task.FromResult(domainValidationException.OperationResult);
        }

        return Task.FromResult(OperationResult.Success());
    }
}
