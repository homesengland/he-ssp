using System.Security.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Programme.Contract;
using AhpProgramme = HE.Investments.Programme.Contract.Programme;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class DraftMilestoneClaim : MilestoneClaimBase
{
    public DraftMilestoneClaim(
        MilestoneType type,
        GrantApportioned grantApportioned,
        ClaimDate claimDate,
        bool? costsIncurred,
        bool? isConfirmed)
        : base(type, MilestoneStatus.Draft, grantApportioned, claimDate, costsIncurred, isConfirmed)
    {
    }

    public override bool IsSubmitted => false;

    public override MilestoneClaimBase WithAchievementDate(
        AchievementDate achievementDate,
        AhpProgramme programme,
        DateDetails? previousSubmissionDate,
        DateTime currentDate)
    {
        ValidateAchievementDate(achievementDate);
        ValidateDateIsNotFuture(achievementDate.Value, currentDate);
        ValidatePreviousSubmissionDate(previousSubmissionDate, achievementDate.Value);
        ValidateProgrammeDates(programme, achievementDate.Value!.Value);

        var newClaimDate = new ClaimDate(ClaimDate.ForecastClaimDate, achievementDate);

        return new DraftMilestoneClaim(Type, GrantApportioned, newClaimDate, CostsIncurred, IsConfirmed);
    }

    public override MilestoneClaimBase WithCostsIncurred(bool? costsIncurred)
    {
        if (Type != MilestoneType.Acquisition)
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                "Costs incurred can be provided only for Acquisition milestone");
        }

        if (costsIncurred.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                ValidationErrorMessage.MustBeSelectedYes(
                    "you have incurred costs and made payments to at least the level of the grant"));
        }

        return new DraftMilestoneClaim(
            Type,
            GrantApportioned,
            ClaimDate,
            costsIncurred,
            IsConfirmed);
    }

    public override MilestoneClaimBase WithConfirmation(bool? isConfirmed)
    {
        if (isConfirmed != true)
        {
            OperationResult.ThrowValidationError(nameof(IsConfirmed), "Confirm the declaration to continue");
        }

        return new DraftMilestoneClaim(Type, GrantApportioned, ClaimDate, CostsIncurred, isConfirmed);
    }

    public override MilestoneWithoutClaim Cancel()
    {
        return new MilestoneWithoutClaim(this);
    }

    private static void ValidateAchievementDate(AchievementDate achievementDate)
    {
        if (achievementDate.IsNotProvided() || achievementDate.Value.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.MustProvideRequiredField("achievement date"));
        }
    }

    private static void ValidateDateIsNotFuture(DateTime? achievementDateTime, DateTime currentDate)
    {
        if (achievementDateTime!.Value.IsAfter(currentDate))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), "The date must be today or in the past");
        }
    }

    private static void ValidatePreviousSubmissionDate(DateDetails? previousSubmissionDate, DateTime? achievementDateTime)
    {
        var previousSubmissionDateTime = DateTimeExtensions.FromDateDetails(previousSubmissionDate);
        if (previousSubmissionDate.IsProvided() && achievementDateTime!.Value.IsBefore(previousSubmissionDateTime!.Value))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }
    }

    private static bool DateWithinRange(DateTime date, DateRange range) => date.IsAfter(range.StartDate) && date.IsBeforeOrEqualTo(range.EndDate);

    private static bool DateWithinProgrammeAndFundingDates(DateTime date, AhpProgramme programme) =>
        DateWithinRange(date, programme.ProgrammeDates) && DateWithinRange(date, programme.FundingDates);

    private static bool DateWithinProgrammeDates(DateTime date, AhpProgramme programme) => DateWithinRange(date, programme.ProgrammeDates);

    private void ValidateProgrammeDates(AhpProgramme programme, DateTime achievementDateTime)
    {
        var isStartOnSite = Type == MilestoneType.StartOnSite && programme.StartOnSiteDates.IsProvided();
        var isCompletion = Type == MilestoneType.Completion && programme.CompletionDates.IsProvided();
        var isAmountNonZero = GrantApportioned.Amount > 0;
        var isAmountZero = GrantApportioned.Amount == 0;

        if (isStartOnSite && !DateWithinRange(achievementDateTime, programme.StartOnSiteDates))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }

        if (isCompletion && !DateWithinRange(achievementDateTime, programme.CompletionDates))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }

        if (isAmountNonZero && !DateWithinProgrammeAndFundingDates(achievementDateTime, programme))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }

        if (isAmountZero && !DateWithinProgrammeDates(achievementDateTime, programme))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }
    }
}
