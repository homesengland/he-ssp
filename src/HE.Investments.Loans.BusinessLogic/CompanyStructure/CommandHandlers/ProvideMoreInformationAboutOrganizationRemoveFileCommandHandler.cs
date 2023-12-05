using HE.Investments.Account.Shared;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationRemoveFileCommand, OperationResult>
{
    private readonly ICompanyStructureRepository _companyStructureRepository;
    private readonly IDocumentService _documentService;
    private readonly ILoansDocumentSettings _documentSettings;
    private readonly INotificationService _notificationService;

    public ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                IAccountUserContext loanUserContext,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IDocumentService documentService,
                ILoansDocumentSettings documentSettings,
                INotificationService notificationService)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _companyStructureRepository = companyStructureRepository;
        _documentService = documentService;
        _documentSettings = documentSettings;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationRemoveFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure =>
            {
                var folderPath = $"{await _companyStructureRepository.GetFilesLocationAsync(request.LoanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}";
                await _documentService.DeleteAsync(
                    new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath),
                    request.FileName,
                    cancellationToken);

                await _notificationService.Publish(new FileRemovedSuccessfullyNotification(request.FileName));
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
