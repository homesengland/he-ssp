using System.Text.Json;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Constants;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
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

    private readonly ICompanyStructureRepository _repository;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository repository,
                ILoanUserContext loanUserContext,
                IDocumentServiceConfig config,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IHttpDocumentService documentService,
                INotificationService notificationService)
        : base(repository, loanUserContext, logger)
    {
        _config = config;
        _documentService = documentService;
        _notificationService = notificationService;
        _loanUserContext = loanUserContext;
        _repository = repository;
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
                        FolderPath = $"{await _repository.GetFilesLocationAsync(request.LoanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}",
                        File = file,
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
                    var valuesToDisplay = new Dictionary<NotificationServiceKeys, string>
                    {
                        { NotificationServiceKeys.Name, filesUploaded[..^2] },
                    };

                    await _notificationService.NotifySuccess(NotificationBodyType.FilesUpload, valuesToDisplay);
                }
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
