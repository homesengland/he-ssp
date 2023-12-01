using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Extensions;
using HE.Investments.Loans.Contract.Funding.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandler : IRequestHandler<CheckAnswersFundingSectionCommand, OperationResult>
{
    private readonly IFundingRepository _fundingRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<FundingBaseCommandHandler> _logger;

    public CheckAnswersFundingSectionCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<FundingBaseCommandHandler> logger)
    {
        _fundingRepository = fundingRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    public async Task<OperationResult> Handle(CheckAnswersFundingSectionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var funding = await _fundingRepository.GetAsync(request.LoanApplicationId, userAccount, FundingFieldsSet.GetAllFields, cancellationToken);

        try
        {
            funding.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer());

            if (funding.Status == SectionStatus.Completed)
            {
                funding.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent(request.LoanApplicationId));
                await _loanApplicationRepository.DispatchEvents(funding, cancellationToken);
            }
            else
            {
                funding.Publish(new LoanApplicationChangeToDraftStatusEvent(funding.LoanApplicationId));
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
