using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Extensions;
using HE.Investments.Loans.Contract.Funding.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;

public class CheckAnswersFundingSectionCommandHandler : IRequestHandler<CheckAnswersFundingSectionCommand, OperationResult>
{
    private readonly IFundingRepository _fundingRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public CheckAnswersFundingSectionCommandHandler(
        IFundingRepository fundingRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _fundingRepository = fundingRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<OperationResult> Handle(CheckAnswersFundingSectionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var funding = await _fundingRepository.GetAsync(request.LoanApplicationId, userAccount, FundingFieldsSet.GetAllFields, cancellationToken);

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

        await _fundingRepository.SaveAsync(funding, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
