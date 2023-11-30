using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureBaseCommandHandler
{
    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<CompanyStructureBaseCommandHandler> _logger;

    public CompanyStructureBaseCommandHandler(
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<CompanyStructureBaseCommandHandler> logger)
    {
        _companyStructureRepository = companyStructureRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(
        Func<CompanyStructureEntity, Task> action,
        LoanApplicationId loanApplicationId,
        CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure =
            await _companyStructureRepository.GetAsync(loanApplicationId, userAccount, CompanyStructureFieldsSet.GetAllFields, cancellationToken);

        try
        {
            await action(companyStructure);

            if (companyStructure.LoanApplicationStatus != ApplicationStatus.Draft)
            {
                companyStructure.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
                await _loanApplicationRepository.DispatchEvents(companyStructure, cancellationToken);
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _companyStructureRepository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
