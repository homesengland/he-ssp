using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Funding.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandler : IRequestHandler<CheckAnswersFundingSectionCommand, OperationResult>
{
    private readonly IFundingRepository _fundingRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<FundingBaseCommandHandler> _logger;

    public CheckAnswersFundingSectionCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
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
