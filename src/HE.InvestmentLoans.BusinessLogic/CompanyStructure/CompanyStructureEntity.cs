using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class CompanyStructureEntity
{
    public CompanyStructureEntity(
        LoanApplicationId loanApplicationId,
        CompanyPurpose? purpose,
        OrganisationMoreInformation? moreInformation,
        OrganisationMoreInformationFile? moreInformationFile,
        HomesBuilt? homesBuilt,
        SectionStatus status,
        ApplicationStatus loanApplicationStatus)
    {
        LoanApplicationId = loanApplicationId;
        Purpose = purpose;
        MoreInformation = moreInformation;
        MoreInformationFile = moreInformationFile;
        HomesBuilt = homesBuilt;
        Status = status;
        LoanApplicationStatus = loanApplicationStatus;
    }

    public CompanyPurpose? Purpose { get; private set; }

    public OrganisationMoreInformation? MoreInformation { get; private set; }

    public OrganisationMoreInformationFile? MoreInformationFile { get; private set; }

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

    public void ProvideFileWithMoreInformation(OrganisationMoreInformationFile? moreInformationFile)
    {
        if (MoreInformationFile == moreInformationFile)
        {
            return;
        }

        MoreInformationFile = moreInformationFile;
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
