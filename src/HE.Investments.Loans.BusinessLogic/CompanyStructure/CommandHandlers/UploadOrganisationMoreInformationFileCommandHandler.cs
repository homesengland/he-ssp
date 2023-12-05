using HE.Investments.Account.Shared;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.DocumentService.Models;
using HE.Investments.DocumentService.Services;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Constants;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Common.Utils;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;

public class UploadOrganisationMoreInformationFileCommandHandler : CompanyStructureBaseCommandHandler,
    IRequestHandler<UploadOrganisationMoreInformationFileCommand, OperationResult<UploadedFile?>>
{
    private readonly ILoansDocumentSettings _documentSettings;

    private readonly IDocumentService _documentService;

    private readonly IMediator _mediator;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    private readonly IDateTimeProvider _dateTimeProvider;

    public UploadOrganisationMoreInformationFileCommandHandler(
        ICompanyStructureRepository companyStructureRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        IMediator mediator,
        IDocumentService documentService,
        ILoansDocumentSettings documentSettings,
        ILogger<CompanyStructureBaseCommandHandler> logger,
        IDateTimeProvider dateTimeProvider)
        : base(companyStructureRepository, loanApplicationRepository, loanUserContext, logger)
    {
        _companyStructureRepository = companyStructureRepository;
        _loanUserContext = loanUserContext;
        _mediator = mediator;
        _documentService = documentService;
        _documentSettings = documentSettings;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<OperationResult<UploadedFile?>> Handle(UploadOrganisationMoreInformationFileCommand request, CancellationToken cancellationToken)
    {
        UploadedFile? uploadedFile = null;
        var result = await Perform(
            async companyStructure =>
            {
                var files = await _mediator.Send(new GetCompanyStructureFilesQuery(request.LoanApplicationId), cancellationToken);
                var filesCount = files.Count + 1;
                companyStructure.ProvideFilesWithMoreInformation(new OrganisationMoreInformationFiles(filesCount));

                if (files.Any(x => x.FileName == request.File.Name))
                {
                    OperationResult.New().AddValidationError("File", GenericValidationError.FileUniqueName).CheckErrors();
                }

                uploadedFile = await UploadFile(request.LoanApplicationId, companyStructure, request.File, cancellationToken);
            },
            request.LoanApplicationId,
            cancellationToken);

        return result.HasValidationErrors ? new OperationResult<UploadedFile?>(result.Errors, null) : new OperationResult<UploadedFile?>(uploadedFile);
    }

    private async Task<UploadedFile> UploadFile(
        LoanApplicationId loanApplicationId,
        CompanyStructureEntity companyStructure,
        FileToUpload file,
        CancellationToken cancellationToken)
    {
        var userDetails = await _loanUserContext.GetProfileDetails();
        var folderPath =
            $"{await _companyStructureRepository.GetFilesLocationAsync(loanApplicationId, cancellationToken)}{CompanyStructureConstants.MoreInformationAboutOrganizationExternal}";
        var fileMetadata = new LoansFileMetadata($"{userDetails.FirstName} {userDetails.LastName}");

        companyStructure.ProvideFileWithMoreInformation(
            new OrganisationMoreInformationFile(file.Name, file.Lenght, _documentSettings.MaxFileSizeInMegabytes));

        await _documentService.UploadAsync(
            new FileLocation(_documentSettings.ListTitle, _documentSettings.ListAlias, folderPath),
            new UploadFileData<LoansFileMetadata>(file.Name, fileMetadata, file.Content),
            true,
            cancellationToken);

        return new UploadedFile(file.Name, _dateTimeProvider.UtcNow, fileMetadata.Creator ?? string.Empty);
    }
}
