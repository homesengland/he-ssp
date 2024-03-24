using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.Helper;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;

public class LoanApplicationEntity : DomainEntity
{
    private const int AllowedFilesCount = 10;

    private IList<UploadedFile>? _files;

    public LoanApplicationEntity(
        LoanApplicationId id,
        LoanApplicationName name,
        UserAccount userAccount,
        ApplicationStatus externalStatus,
        FundingPurpose fundingReason,
        DateTime? createdOn,
        DateTime? lastModificationDate,
        DateTime? submittedOn,
        string lastModifiedBy,
        LoanApplicationSection companyStructure,
        LoanApplicationSection security,
        LoanApplicationSection funding,
        ProjectsSection projectsSection,
        string referenceNumber,
        FrontDoorProjectId? frontDoorProjectId)
    {
        Id = id;
        Name = name;
        UserAccount = userAccount;
        ExternalStatus = externalStatus;
        LastModificationDate = lastModificationDate;
        SubmittedOn = submittedOn;
        LastModifiedBy = lastModifiedBy;
        FundingReason = fundingReason;
        CreatedOn = createdOn;
        CompanyStructure = companyStructure;
        Security = security;
        Funding = funding;
        ProjectsSection = projectsSection;
        ReferenceNumber = referenceNumber;
        FrontDoorProjectId = frontDoorProjectId;
    }

    public LoanApplicationId Id { get; private set; }

    public FrontDoorProjectId? FrontDoorProjectId { get; private set; }

    public LoanApplicationName Name { get; }

    public UserAccount UserAccount { get; }

    public LoanApplicationSection CompanyStructure { get; private set; }

    public LoanApplicationSection Security { get; private set; }

    public LoanApplicationSection Funding { get; private set; }

    public ProjectsSection ProjectsSection { get; private set; }

    public ApplicationStatus ExternalStatus { get; set; }

    public DateTime? LastModificationDate { get; }

    public string LastModifiedBy { get; }

    public DateTime? CreatedOn { get; }

    public DateTime? SubmittedOn { get; }

    public FundingPurpose FundingReason { get; private set; }

    public string ReferenceNumber { get; private set; }

    public static LoanApplicationEntity New(UserAccount userAccount, LoanApplicationName name, FrontDoorProjectId? frontDoorProjectId) => new(LoanApplicationId.New(), name, userAccount, ApplicationStatus.Draft, FundingPurpose.BuildingNewHomes, null, null, null, string.Empty, LoanApplicationSection.New(), LoanApplicationSection.New(), LoanApplicationSection.New(), ProjectsSection.Empty(), string.Empty, frontDoorProjectId);

    public void SetId(LoanApplicationId newId)
    {
        if (Id.IsSaved())
        {
            throw new DomainException("Id cannot be modified", CommonErrorCodes.IdCannotBeModified);
        }

        Id = newId;
        Publish(new LoanApplicationHasBeenStartedEvent(Id, Name.Value, FrontDoorProjectId?.Value));
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(ExternalStatus);
    }

    public async Task<IReadOnlyCollection<UploadedFile>> UploadFiles(
        ILoansFileService<SupportingDocumentsParams> fileService,
        IList<SupportingDocumentsFile> filesToUpload,
        CancellationToken cancellationToken)
    {
        _files ??= (await fileService.GetFiles(SupportingDocumentsParams.New(Id), cancellationToken)).ToList();

        if (!_files.Any()) // todo
        {
            return Array.Empty<UploadedFile>();
        }

        if (_files.Count + filesToUpload.Count > AllowedFilesCount)
        {
            OperationResult.ThrowValidationError(nameof(SupportingDocumentsFile), ValidationErrorMessage.FilesMaxCount(AllowedFilesCount));
        }

        var isNameDuplicated = _files!.Select(x => x.Name)
            .Concat(filesToUpload.Select(x => x.FileName))
            .GroupBy(x => x)
            .Any(x => x.Count() > 1);
        if (isNameDuplicated)
        {
            OperationResult.ThrowValidationError(nameof(SupportingDocumentsFile), GenericValidationError.FileWithThatNameExistsRename);
        }

        var result = new List<UploadedFile>();
        foreach (var fileToUpload in filesToUpload)
        {
            result.Add(await fileService.UploadFile(fileToUpload.FileName, fileToUpload.FileContent, SupportingDocumentsParams.New(Id), cancellationToken));
        }

        _files.AddRange(result);
        Publish(new ApplicationFilesUploadedSuccessfullyEvent(_files.Count));

        return result;
    }

    public async Task Submit(ICanSubmitLoanApplication canSubmitLoanApplication, CancellationToken cancellationToken)
    {
        CheckIfCanBeSubmitted();

        await canSubmitLoanApplication.Submit(Id, cancellationToken);
    }

    public bool CanBeSubmitted()
    {
        return IsReadyToSubmit() && !IsSubmitted();
    }

    public bool WasSubmitted() => SubmittedOn != null;

    public void CheckIfCanBeSubmitted()
    {
        if (!IsReadyToSubmit())
        {
            throw new DomainException("Loan application is not ready to be submitted", CommonErrorCodes.LoanApplicationNotReadyToSubmit);
        }

        if (IsSubmitted())
        {
            throw new DomainException(
                "Loan application has been submitted",
                CommonErrorCodes.ApplicationHasBeenSubmitted,
                ("Date", LastModificationDate!.Value));
        }
    }

    public async Task Withdraw(ILoanApplicationRepository loanApplicationRepository, WithdrawReason withdrawReason, CancellationToken cancellationToken)
    {
        var statusesAfterSubmit = ApplicationStatusDivision.GetAllStatusesAfterSubmit();

        if (ExternalStatus == ApplicationStatus.Draft)
        {
            await loanApplicationRepository.WithdrawDraft(Id, withdrawReason, cancellationToken);
        }
        else if (statusesAfterSubmit.Contains(ExternalStatus))
        {
            await loanApplicationRepository.WithdrawSubmitted(Id, withdrawReason, cancellationToken);
        }
        else
        {
            throw new DomainException("Loan application cannot be withdrawn", CommonErrorCodes.ApplicationCannotBeWithdrawn);
        }

        Publish(new LoanApplicationHasBeenWithdrawnEvent(Id, Name));
    }

    public bool IsEnoughHomesToBuild()
    {
        const int minimumHomesToBuild = 5;

        return ProjectsSection.TotalHomesBuilt() >= minimumHomesToBuild;
    }

    private bool IsReadyToSubmit()
    {
        return ProjectsSection.IsCompleted() && Funding.IsCompleted() && Security.IsCompleted() && CompanyStructure.IsCompleted();
    }

    private bool IsSubmitted()
    {
        return ExternalStatus == ApplicationStatus.ApplicationSubmitted;
    }
}
