using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.FinancialDetails.CommandHandlers;

public abstract class FinancialDetailsCommandHandlerBase
{
    private readonly IFinancialDetailsRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly ILogger<FinancialDetailsCommandHandlerBase> _logger;

    protected FinancialDetailsCommandHandlerBase(IFinancialDetailsRepository repository, IAccountUserContext accountUserContext, ILogger<FinancialDetailsCommandHandlerBase> logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(Action<FinancialDetailsEntity> action, AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var financialDetails = await _repository.GetById(applicationId, account, cancellationToken);

        try
        {
            action(financialDetails);
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _repository.Save(financialDetails, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
