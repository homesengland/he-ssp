using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Extensions;
using HE.Investments.Loans.Contract.Security.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;

public class CheckSecurityAnswersCommandHandler : IRequestHandler<ConfirmSecuritySectionCommand, OperationResult>
{
    private readonly ISecurityRepository _securityRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public CheckSecurityAnswersCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _securityRepository = securityRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<OperationResult> Handle(ConfirmSecuritySectionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var security = await _securityRepository.GetAsync(request.Id, userAccount, SecurityFieldsSet.GetAllFields, cancellationToken);

        security.CheckAnswers(request.Answer.ToYesNoAnswer());

        if (security.Status == SectionStatus.Completed)
        {
            security.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent(request.Id));
            await _loanApplicationRepository.DispatchEvents(security, cancellationToken);
        }
        else
        {
            security.Publish(new LoanApplicationChangeToDraftStatusEvent(security.LoanApplicationId));
            await _loanApplicationRepository.DispatchEvents(security, cancellationToken);
        }

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
