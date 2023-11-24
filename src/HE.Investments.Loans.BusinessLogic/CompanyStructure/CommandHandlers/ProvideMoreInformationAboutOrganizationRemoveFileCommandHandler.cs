using HE.Investments.Account.Shared;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationRemoveFileCommand, OperationResult>
{
    private readonly IHttpDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;
    private readonly INotificationService _notificationService;

    public ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                IAccountUserContext loanUserContext,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                IDocumentServiceConfig config,
                INotificationService notificationService)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _documentService = documentService;
        _config = config;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationRemoveFileCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure =>
            {
                await _documentService.DeleteAsync(
                    _config.ListAlias,
                    request.FolderPath,
                    request.FileName);

                await _notificationService.Publish(new FileRemovedSuccessfullyNotification(request.FileName));
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
