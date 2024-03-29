using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Security.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Security;

public class SecurityEntity : DomainEntity
{
    public SecurityEntity(
        LoanApplicationId applicationId,
        Debenture debenture,
        DirectorLoans directLoans,
        DirectorLoansSubordinate directorLoansSubordinate,
        SectionStatus status,
        ApplicationStatus loanApplicationStatus)
    {
        LoanApplicationId = applicationId;
        Debenture = debenture;
        DirectorLoans = directLoans;
        Status = status;
        DirectorLoansSubordinate = directorLoansSubordinate;
        LoanApplicationStatus = loanApplicationStatus;
    }

    public LoanApplicationId LoanApplicationId { get; private set; }

    public ApplicationStatus LoanApplicationStatus { get; }

    public Debenture Debenture { get; private set; }

    public DirectorLoans DirectorLoans { get; private set; }

    public DirectorLoansSubordinate DirectorLoansSubordinate { get; private set; }

    public SectionStatus Status { get; private set; }

    public void ProvideDebenture(Debenture debenture)
    {
        if (Debenture == debenture)
        {
            return;
        }

        Debenture = debenture;
        UncompleteSection();
    }

    public void ProvideDirectorLoans(DirectorLoans directLoans)
    {
        if (DirectorLoans == directLoans)
        {
            return;
        }

        DirectorLoans = directLoans;

        if (DirectorLoansDoNotExists())
        {
            DirectorLoansSubordinate = null!;
        }

        UncompleteSection();
    }

    public void ProvideDirectorLoansSubordinate(DirectorLoansSubordinate directLoansSubordinate)
    {
        if (DirectorLoansDoNotExists())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(DirectorLoans), ValidationErrorMessage.DirectorLoansDoesNotExist)
                .CheckErrors();
        }

        DirectorLoansSubordinate = directLoansSubordinate;

        UncompleteSection();
    }

    internal void CheckAnswers(YesNoAnswers answer)
    {
        switch (answer)
        {
            case YesNoAnswers.Yes:
                if (Debenture.IsNotProvided() || DirectorLoans.IsNotProvided() || (DirectorLoans.Exists && DirectorLoansSubordinate.IsNotProvided()))
                {
                    OperationResult.New().AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.CheckAnswersOption).CheckErrors();
                }

                CompleteSection();
                break;
            case YesNoAnswers.No:
                UncompleteSection();
                break;
            case YesNoAnswers.Undefined:
                OperationResult.New()
                    .AddValidationError(nameof(CheckAnswers), ValidationErrorMessage.NoCheckAnswers)
                    .CheckErrors();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(answer), answer, null);
        }
    }

    private bool DirectorLoansDoNotExists()
    {
        return DirectorLoans.IsNotProvided() || !DirectorLoans.Exists;
    }

    private void CompleteSection()
    {
        Status = SectionStatus.Completed;
    }

    private void UncompleteSection()
    {
        Status = SectionStatus.InProgress;
    }
}
