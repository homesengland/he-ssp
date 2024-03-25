using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Commands;
using MediatR;
using INotificationPublisher = HE.Investments.Common.Services.Notifications.INotificationPublisher;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class RemoveSupportingDocumentsFileCommandHandler : ApplicationBaseCommandHandler,
    IRequestHandler<RemoveSupportingDocumentsFileCommand, OperationResult>
{
    private readonly ILoansFileService<SupportingDocumentsParams> _fileService;
    private readonly INotificationPublisher _notificationPublisher;

    public RemoveSupportingDocumentsFileCommandHandler(
                ILoanApplicationRepository loanApplicationRepository,
                ILoansFileService<SupportingDocumentsParams> fileService,
                IAccountUserContext loanUserContext,
                INotificationPublisher notificationPublisher)
        : base(loanApplicationRepository, loanUserContext)
    {
        _notificationPublisher = notificationPublisher;
        _fileService = fileService;
    }

    public async Task<OperationResult> Handle(RemoveSupportingDocumentsFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async _ =>
            {
                var removedFile = await _fileService.RemoveFile(request.FileId, SupportingDocumentsParams.New(request.LoanApplicationId), cancellationToken);
                if (removedFile.IsProvided())
                {
                    await _notificationPublisher.Publish(new FileRemovedSuccessfullyNotification(removedFile!.Name));
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
