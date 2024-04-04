using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.PrefillData.Data;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
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
    private readonly ILoanPrefillDataRepository _prefillDataRepository;

    public StartApplicationCommandHandler(
        IAccountUserContext loanUserContext,
        ILoanApplicationRepository applicationRepository,
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanPrefillDataRepository prefillDataRepository)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
        _applicationProjectsRepository = applicationProjectsRepository;
        _prefillDataRepository = prefillDataRepository;
    }

    public async Task<OperationResult<LoanApplicationId?>> Handle(StartApplicationCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var prefillData = await GetPrefillData(request, userAccount, cancellationToken);

        var applicationName = new LoanApplicationName(request.ApplicationName);
        var newLoanApplication = LoanApplicationEntity.New(userAccount, applicationName, prefillData?.ProjectId);

        if (await _applicationRepository.IsExist(applicationName, userAccount, cancellationToken))
        {
            return new OperationResult<LoanApplicationId?>(
                new[] { new ErrorItem(nameof(LoanApplicationName), "This name has been used for another application") },
                null);
        }

        await _applicationRepository.Save(newLoanApplication, await _loanUserContext.GetProfileDetails(), cancellationToken);

        var applicationProjects = new ApplicationProjects(newLoanApplication.Id, prefillData?.SiteId);
        await _applicationProjectsRepository.SaveAllAsync(applicationProjects, userAccount, cancellationToken);

        await _applicationRepository.DispatchEvents(newLoanApplication, cancellationToken);

        return OperationResult.Success<LoanApplicationId?>(newLoanApplication.Id);
    }

    private async Task<LoanApplicationPrefillData?> GetPrefillData(StartApplicationCommand request, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FrontDoorProjectId))
        {
            return null;
        }

        var frontDoorProjectId = new FrontDoorProjectId(request.FrontDoorProjectId);
        return await _prefillDataRepository.GetLoanApplicationPrefillData(frontDoorProjectId, userAccount, cancellationToken);
    }
}
