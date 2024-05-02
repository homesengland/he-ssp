using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.Events;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using SectionStatus = HE.Investments.Common.Contract.SectionStatus;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity : DomainEntity
{
    private const int AllowedFilesCount = 10;

    private readonly IEventDispatcher _eventDispatcher;

    private IList<UploadedFile>? _files;

    public CompanyStructureEntity(
        LoanApplicationId loanApplicationId,
        CompanyPurpose? purpose,
        OrganisationMoreInformation? moreInformation,
        HomesBuilt? homesBuilt,
        SectionStatus status,
        ApplicationStatus loanApplicationStatus,
        IEventDispatcher eventDispatcher)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
        MoreInformation = moreInformation;
        HomesBuilt = homesBuilt;
        Status = status;
        LoanApplicationStatus = loanApplicationStatus;
        _eventDispatcher = eventDispatcher;
    }

    public CompanyPurpose? Purpose { get; private set; }

    public OrganisationMoreInformation? MoreInformation { get; private set; }

    public HomesBuilt? HomesBuilt { get; private set; }

    public SectionStatus Status { get; private set; }

    public LoanApplicationId LoanApplicationId { get; }

    public ApplicationStatus LoanApplicationStatus { get; }

    public void ProvideCompanyPurpose(CompanyPurpose? purpose)
    {
        if (Purpose == purpose)
        {
            return;
        }

        Purpose = purpose;
        UnCompleteSection();
    }

    public void ProvideMoreInformation(OrganisationMoreInformation? moreInformation)
    {
        if (MoreInformation == moreInformation)
        {
            return;
        }

        MoreInformation = moreInformation;
        UnCompleteSection();
    }

    public async Task<IReadOnlyCollection<UploadedFile>> UploadFiles(
        ILoansFileService<LoanApplicationId> fileService,
        IList<OrganisationMoreInformationFile> filesToUpload,
        CancellationToken cancellationToken)
    {
        if (!filesToUpload.Any())
        {
            return [];
        }

        _files ??= (await fileService.GetFiles(LoanApplicationId, cancellationToken)).ToList();
        if (_files.Count + filesToUpload.Count > AllowedFilesCount)
        {
            OperationResult.ThrowValidationError(nameof(OrganisationMoreInformationFile), ValidationErrorMessage.FilesMaxCount(AllowedFilesCount));
        }

        var isNameDuplicated = _files!.Select(x => x.Name)
            .Concat(filesToUpload.Select(x => x.FileName))
            .GroupBy(x => x)
            .Any(x => x.Count() > 1);
        if (isNameDuplicated)
        {
            OperationResult.ThrowValidationError(nameof(OrganisationMoreInformationFile), GenericValidationError.FileUniqueName);
        }

        var result = new List<UploadedFile>();
        foreach (var fileToUpload in filesToUpload)
        {
            result.Add(await fileService.UploadFile(fileToUpload.FileName, fileToUpload.FileContent, LoanApplicationId, cancellationToken));
        }

        UnCompleteSection();
        _files.AddRange(result);
        await _eventDispatcher.Publish(new FilesUploadedSuccessfullyEvent(result.Select(x => x.Name).ToList()), cancellationToken);

        return result;
    }

    public void ProvideHowManyHomesBuilt(HomesBuilt? homesBuilt)
    {
        if (HomesBuilt == homesBuilt)
        {
            return;
        }

        HomesBuilt = homesBuilt;
        UnCompleteSection();
    }

    public void CheckAnswers(YesNoAnswers yesNoAnswers)
    {
        switch (yesNoAnswers)
        {
            case YesNoAnswers.Yes:
                CompleteSection();
                break;
            case YesNoAnswers.No:
                UnCompleteSection();
                break;
            case YesNoAnswers.Undefined:
                OperationResult.New()
                    .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.NoCheckAnswers)
                    .CheckErrors();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(yesNoAnswers), yesNoAnswers, null);
        }
    }

    private void CompleteSection()
    {
        if (HomesBuilt.IsNotProvided() ||
            MoreInformation.IsNotProvided() ||
            Purpose.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.CheckAnswersOption)
                .CheckErrors();
        }

        Status = SectionStatus.Completed;
    }

    private void UnCompleteSection()
    {
        Status = SectionStatus.InProgress;
    }
}
