using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationRemoveFileCommand, OperationResult>
{
    private readonly IHttpDocumentService _documentService;
    private readonly IDocumentServiceConfig _config;
    private readonly INotificationService _notificationService;

    public ProvideMoreInformationAboutOrganizationRemoveFileCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                IDocumentServiceConfig config,
                INotificationService notificationService)
        : base(repository, loanUserContext, logger)
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

                var valuesToDisplay = new Dictionary<NotificationServiceKeys, string>
                    {
                        { NotificationServiceKeys.Name, request.FileName },
                    };

                await _notificationService.NotifySuccess(NotificationBodyType.FileRemove, valuesToDisplay);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
