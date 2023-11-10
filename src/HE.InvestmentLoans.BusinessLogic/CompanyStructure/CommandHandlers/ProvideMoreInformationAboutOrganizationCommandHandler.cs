using System.Text.Json;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Notifications;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Configs;
using HE.Investments.DocumentService.Models.File;
using HE.Investments.DocumentService.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly IDocumentServiceConfig _config;

    private readonly IHttpDocumentService _documentService;

    private readonly INotificationService _notificationService;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                ILoanUserContext loanUserContext,
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

    public async Task<OperationResult> Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async companyStructure =>
            {
                companyStructure.ProvideMoreInformation(
                    request.OrganisationMoreInformation.IsProvided() ? new OrganisationMoreInformation(request.OrganisationMoreInformation!) : null);

                if (request.FormFiles == null)
                {
                    return;
                }

                var filesCount = request.OrganisationMoreInformationFiles?.Count + request.FormFiles.Count;
                companyStructure.ProvideFilesWithMoreInformation(new OrganisationMoreInformationFiles(filesCount));

                var filesUploaded = string.Empty;
                var userDetails = await _loanUserContext.GetUserDetails();

                foreach (var formFile in request.FormFiles)
                {
                    var file = new FileData(formFile);
                    companyStructure.ProvideFileWithMoreInformation(new OrganisationMoreInformationFile(file.Name, file.Data, _config.MaxFileSizeInMegabytes));

                    await _documentService.UploadAsync(new FileUploadModel()
                    {
                        ListTitle = _config.ListTitle,
                        FolderPath = $"{await _companyStructureRepository.GetFilesLocationAsync(request.LoanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
                        File = formFile,
                        Metadata = JsonSerializer.Serialize(new FileMetadata
                        {
                            Creator = $"{userDetails.FirstName} {userDetails.LastName}",
                        }),
                        Overwrite = true,
                    });

                    filesUploaded += $"{file.Name}, ";
                }

                if (!string.IsNullOrEmpty(filesUploaded))
                {
                    await _notificationService.Publish(new FilesUploadedSuccessfullyNotification(filesUploaded[..^2]));
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
