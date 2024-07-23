using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;
using INotificationPublisher = HE.Investments.Common.Services.Notifications.INotificationPublisher;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class RemoveMoreInformationAboutOrganizationFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<RemoveMoreInformationAboutOrganizationFileCommand, OperationResult>
{
    private readonly ILoansFileService<CompanyStructureFileParams> _companyStructureFileService;
    private readonly INotificationPublisher _notificationPublisher;

    public RemoveMoreInformationAboutOrganizationFileCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                ILoansFileService<CompanyStructureFileParams> companyStructureFileService,
                IAccountUserContext loanUserContext,
                INotificationPublisher notificationPublisher)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext)
    {
        _notificationPublisher = notificationPublisher;
        _companyStructureFileService = companyStructureFileService;
    }

    public async Task<OperationResult> Handle(RemoveMoreInformationAboutOrganizationFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async _ =>
            {
                var removedFile = await _companyStructureFileService.RemoveFile(request.FileId, new CompanyStructureFileParams(request.LoanApplicationId), cancellationToken);
                if (removedFile.IsProvided())
                {
                    await _notificationPublisher.Publish(new FileRemovedSuccessfullyNotification(removedFile!.Name));
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
