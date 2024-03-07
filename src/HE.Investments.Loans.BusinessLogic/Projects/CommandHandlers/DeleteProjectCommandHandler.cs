using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.Projects.Notifications;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using INotificationPublisher = HE.Investments.Common.Services.Notifications.INotificationPublisher;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, OperationResult>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;
    private readonly IAccountUserContext _loanUserContext;
    private readonly INotificationPublisher _notificationPublisher;

    public DeleteProjectCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        IAccountUserContext loanUserContext,
        INotificationPublisher notificationPublisher)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _loanUserContext = loanUserContext;
        _notificationPublisher = notificationPublisher;
    }

    public async Task<OperationResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();

        var applicationProjects =
            await _applicationProjectsRepository.GetAllAsync(request.LoanApplicationId, userAccount, cancellationToken);

        var deletedProject = applicationProjects.Remove(request.ProjectId);

        await _applicationProjectsRepository.SaveAsync(request.LoanApplicationId, deletedProject, cancellationToken);
        await _notificationPublisher.Publish(new ProjectDeletedSuccessfullyNotification(deletedProject.Name?.Value ?? ProjectName.Default.Value));

        return OperationResult.Success();
    }
}
