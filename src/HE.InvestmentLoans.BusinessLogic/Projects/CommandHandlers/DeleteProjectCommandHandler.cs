using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
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
        var valuesToDisplay = new Dictionary<NotificationServiceKeys, string>
        {
            { NotificationServiceKeys.Name, deletedProject.Name?.Value ?? ProjectName.Default.Value },
        };

        await _notificationService.NotifySuccess(NotificationBodyType.DeleteProject, valuesToDisplay);

        await _applicationProjectsRepository.SaveAsync(request.LoanApplicationId, deletedProject, cancellationToken);

        return OperationResult.Success();
    }
}
