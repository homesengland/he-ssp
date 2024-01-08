using System.Text;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Notifications;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand, OperationResult>
{
    private readonly ILoansDocumentSettings _documentSettings;

    private readonly IDocumentService _documentService;

    private readonly INotificationService _notificationService;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    public ProvideMoreInformationAboutOrganizationCommandHandler(
                ICompanyStructureRepository companyStructureRepository,
                ILoanApplicationRepository loanApplicationRepository,
                IAccountUserContext loanUserContext,
                ILoansDocumentSettings documentSettings,
                ILogger<CompanyStructureBaseCommandHandler> logger,
                IDocumentService documentService,
                INotificationService notificationService)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _documentSettings = documentSettings;
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

                await UploadFiles(request.LoanApplicationId, companyStructure, request.FormFiles, cancellationToken);
            },
            request.LoanApplicationId,
            cancellationToken);
    }

    private async Task UploadFiles(
        LoanApplicationId loanApplicationId,
        CompanyStructureEntity companyStructure,
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken)
    {
        var bld = new StringBuilder();

        var userDetails = await _loanUserContext.GetProfileDetails();
        var folderPath = $"{await _companyStructureRepository.GetFilesLocationAsync(loanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}";
        var fileMetadata = new LoansFileMetadata($"{userDetails.FirstName} {userDetails.LastName}");

        foreach (var file in files)
        {
            companyStructure.ProvideFileWithMoreInformation(new OrganisationMoreInformationFile(file.FileName, file.Length, _documentSettings.MaxFileSizeInMegabytes));

            await using var fileStream = file.OpenReadStream();
            await _documentService.UploadAsync(
                new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath),
                new UploadFileData<LoansFileMetadata>(file.FileName, fileMetadata, fileStream),
                true,
                cancellationToken);

            bld.Append($"{file.FileName}, ");
        }

        var filesUploaded = bld.ToString();

        if (!string.IsNullOrEmpty(filesUploaded))
        {
            await _notificationService.Publish(new FilesUploadedSuccessfullyNotification(filesUploaded[..^2]));
        }
    }
}
