using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Extensions;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CheckAnswersCompanyStructureSectionCommandHandler : IRequestHandler<CheckAnswersCompanyStructureSectionCommand, OperationResult>
{
    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public CheckAnswersCompanyStructureSectionCommandHandler(
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _companyStructureRepository = companyStructureRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<OperationResult> Handle(CheckAnswersCompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure =
            await _companyStructureRepository.GetAsync(request.LoanApplicationId, userAccount, CompanyStructureFieldsSet.GetAllFields, cancellationToken);

        companyStructure.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer());
        if (companyStructure.Status == SectionStatus.Completed)
        {
            companyStructure.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent(request.LoanApplicationId));
            await _loanApplicationRepository.DispatchEvents(companyStructure, cancellationToken);
        }
        else
        {
            companyStructure.Publish(new LoanApplicationChangeToDraftStatusEvent(companyStructure.LoanApplicationId));
            await _loanApplicationRepository.DispatchEvents(companyStructure, cancellationToken);
        }

        await _companyStructureRepository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
