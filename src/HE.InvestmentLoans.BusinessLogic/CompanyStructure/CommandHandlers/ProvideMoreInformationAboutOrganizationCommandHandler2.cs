using System.Text.Json;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler2 : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand2, OperationResult>
{
    private readonly IDocumentServiceConfig _config;

    private readonly IHttpDocumentService _documentService;

    private readonly INotificationService _notificationService;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    public ProvideMoreInformationAboutOrganizationCommandHandler2(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                IAccountUserContext loanUserContext,
                IDocumentServiceConfig config,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                INotificationService notificationService)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _config = config;
        _documentService = documentService;
        _notificationService = notificationService;
        _loanUserContext = loanUserContext;
        _companyStructureRepository = companyStructureRepository;
    }

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand2 request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure =>
            {
                await UploadFiles(request.LoanApplicationId, companyStructure, request.Files, cancellationToken);
            },
            request.LoanApplicationId,
            cancellationToken);
    }

    private async Task UploadFiles(
        LoanApplicationId loanApplicationId,
        CompanyStructureEntity companyStructure,
        IAsyncEnumerable<LargeFile> files,
        CancellationToken cancellationToken)
    {
        var filesUploaded = string.Empty;
        var userDetails = await _loanUserContext.GetProfileDetails();
        var folderPath = $"{await _companyStructureRepository.GetFilesLocationAsync(loanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}";
        var fileMetadata = JsonSerializer.Serialize(new FileMetadata { Creator = $"{userDetails.FirstName} {userDetails.LastName}" });

        await foreach (var file in files.WithCancellation(cancellationToken))
        {
            // companyStructure.ProvideFileWithMoreInformation(new OrganisationMoreInformationFile(file.FileName, file.Length, _config.MaxFileSizeInMegabytes));
            await _documentService.UploadAsync(new FileUploadModel
            {
                ListTitle = _config.ListTitle,
                FolderPath = folderPath,
                FileStream = file.Content,
                Metadata = fileMetadata,
                Overwrite = true,
                FileName = file.Name,
            });

            filesUploaded += $"{file.Name}, ";
        }

        if (!string.IsNullOrEmpty(filesUploaded))
        {
            await _notificationService.Publish(new FilesUploadedSuccessfullyNotification(filesUploaded[..^2]));
        }
    }
}
