using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureBaseCommandHandler
{
    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public CompanyStructureBaseCommandHandler(
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _companyStructureRepository = companyStructureRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(
        Func<CompanyStructureEntity, Task> action,
        LoanApplicationId loanApplicationId,
        CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure =
            await _companyStructureRepository.GetAsync(loanApplicationId, userAccount, CompanyStructureFieldsSet.GetAllFields, cancellationToken);

        await action(companyStructure);

        if (companyStructure.LoanApplicationStatus != ApplicationStatus.Draft)
        {
            companyStructure.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
            await _loanApplicationRepository.DispatchEvents(companyStructure, cancellationToken);
        }

        await _companyStructureRepository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
