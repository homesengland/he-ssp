using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
public class FundingBaseCommandHandler
{
    private readonly IFundingRepository _fundingRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<FundingBaseCommandHandler> _logger;

    public FundingBaseCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
    {
        _fundingRepository = fundingRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(Action<FundingEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var funding = await _fundingRepository.GetAsync(loanApplicationId, userAccount, FundingFieldsSet.GetAllFields, cancellationToken);

        try
        {
            action(funding);

            if (funding.LoanApplicationStatus != ApplicationStatus.Draft)
            {
                funding.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
                await _loanApplicationRepository.DispatchEvents(funding, cancellationToken);
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _fundingRepository.SaveAsync(funding, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
