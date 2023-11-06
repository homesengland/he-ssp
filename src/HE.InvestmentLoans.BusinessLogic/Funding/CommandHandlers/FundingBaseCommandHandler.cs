using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class FundingBaseCommandHandler
{
    private readonly IFundingRepository _fundingRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<FundingBaseCommandHandler> _logger;

    public FundingBaseCommandHandler(IFundingRepository fundingRepository, ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ILogger<FundingBaseCommandHandler> logger)
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
                funding.Publish(new LoanApplicationIsInDraftStatusEvent(loanApplicationId));
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
