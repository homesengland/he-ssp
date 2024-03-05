using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandler : IRequestHandler<StartApplicationCommand, OperationResult<LoanApplicationId?>>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    public StartApplicationCommandHandler(
        IAccountUserContext loanUserContext,
        ILoanApplicationRepository applicationRepository,
        IApplicationProjectsRepository applicationProjectsRepository)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
        _applicationProjectsRepository = applicationProjectsRepository;
    }

    public async Task<OperationResult<LoanApplicationId?>> Handle(StartApplicationCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var applicationName = new LoanApplicationName(request.ApplicationName);
        var newLoanApplication = LoanApplicationEntity.New(userAccount, applicationName);

        if (await _applicationRepository.IsExist(applicationName, userAccount, cancellationToken))
        {
            return new OperationResult<LoanApplicationId?>(
                new[] { new ErrorItem(nameof(LoanApplicationName), "This name has been used for another application") },
                null);
        }

        await _applicationRepository.Save(newLoanApplication, await _loanUserContext.GetProfileDetails(), cancellationToken);

        var applicationProjects = new ApplicationProjects(newLoanApplication.Id);
        await _applicationProjectsRepository.SaveAllAsync(applicationProjects, userAccount, cancellationToken);

        await _applicationRepository.DispatchEvents(newLoanApplication, cancellationToken);

        return OperationResult.Success<LoanApplicationId?>(newLoanApplication.Id);
    }
}
