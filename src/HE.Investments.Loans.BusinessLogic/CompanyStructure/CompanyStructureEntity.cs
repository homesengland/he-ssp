using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using SectionStatus = HE.Investments.Common.Contract.SectionStatus;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity : DomainEntity
{
    public CompanyStructureEntity(
        LoanApplicationId loanApplicationId,
        CompanyPurpose? purpose,
        OrganisationMoreInformation? moreInformation,
        HomesBuilt? homesBuilt,
        SectionStatus status,
        ApplicationStatus loanApplicationStatus)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
        MoreInformation = moreInformation;
        HomesBuilt = homesBuilt;
        Status = status;
        LoanApplicationStatus = loanApplicationStatus;
    }

    public CompanyPurpose? Purpose { get; private set; }

    public OrganisationMoreInformation? MoreInformation { get; private set; }

    public OrganisationMoreInformationFile? MoreInformationFile { get; private set; }

    public OrganisationMoreInformationFiles? MoreInformationFiles { get; private set; }

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

    public void ProvideFileWithMoreInformation(OrganisationMoreInformationFile moreInformationFile)
    {
        if (MoreInformationFile == moreInformationFile)
        {
            return;
        }

        MoreInformationFile = moreInformationFile;
        UnCompleteSection();
    }

    public void ProvideFilesWithMoreInformation(OrganisationMoreInformationFiles moreInformationFiles)
    {
        if (MoreInformationFiles == moreInformationFiles)
        {
            return;
        }

        MoreInformationFiles = moreInformationFiles;
        UnCompleteSection();
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
