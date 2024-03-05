using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class RemoveMoreInformationAboutOrganizationFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<RemoveMoreInformationAboutOrganizationFileCommand, OperationResult>
{
    private readonly ILoansFileService<LoanApplicationId> _companyStructureFileService;
    private readonly INotificationService _notificationService;

    public RemoveMoreInformationAboutOrganizationFileCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                ILoansFileService<LoanApplicationId> companyStructureFileService,
                IAccountUserContext loanUserContext,
                INotificationService notificationService)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext)
    {
        _notificationService = notificationService;
        _companyStructureFileService = companyStructureFileService;
    }

    public async Task<OperationResult> Handle(RemoveMoreInformationAboutOrganizationFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async _ =>
            {
                var removedFile = await _companyStructureFileService.RemoveFile(request.FileId, request.LoanApplicationId, cancellationToken);
                if (removedFile.IsProvided())
                {
                    await _notificationService.Publish(new FileRemovedSuccessfullyNotification(removedFile!.Name));
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
