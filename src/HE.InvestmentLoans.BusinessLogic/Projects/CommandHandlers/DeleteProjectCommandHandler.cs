using HE.InvestmentLoans.BusinessLogic.Projects.Notifications;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, OperationResult>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly ILoanUserContext _loanUserContext;
    private readonly INotificationService _notificationService;

    public DeleteProjectCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanUserContext loanUserContext,
        INotificationService notificationService)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var applicationProjects =
            await _applicationProjectsRepository.GetAllAsync(request.LoanApplicationId, userAccount, cancellationToken);

        var deletedProject = applicationProjects.Remove(request.ProjectId);

        await _applicationProjectsRepository.SaveAsync(request.LoanApplicationId, deletedProject, cancellationToken);
        await _notificationService.Publish(new ProjectDeletedSuccessfullyNotification(deletedProject.Name?.Value ?? ProjectName.Default.Value));

        return OperationResult.Success();
    }
}
