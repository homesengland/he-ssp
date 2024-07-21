using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Programme.Contract;
using AhpProgramme = HE.Investments.Programme.Contract.Programme;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class MilestoneClaim : ValueObject
{
    public MilestoneClaim(
        MilestoneType type,
        MilestoneStatus status,
        GrantApportioned grantApportioned,
        ClaimDate claimDate,
        bool? costsIncurred,
        bool? isConfirmed)
    {
        if (type != MilestoneType.Acquisition && costsIncurred.IsProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                "Costs incurred can be provided only for Acquisition milestone");
        }

        Type = type;
        Status = status;
        GrantApportioned = grantApportioned;
        ClaimDate = claimDate;
        CostsIncurred = costsIncurred;
        IsConfirmed = isConfirmed;
    }

    public MilestoneType Type { get; }

    public MilestoneStatus Status { get; }

    public GrantApportioned GrantApportioned { get; }

    public ClaimDate ClaimDate { get; }

    public bool? CostsIncurred { get; }

    public bool? IsConfirmed { get; }

    public bool IsSubmitted => Status >= MilestoneStatus.Submitted;

    public MilestoneDueStatus CalculateDueStatus(DateTime today)
    {
        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(14)))
        {
            return MilestoneDueStatus.Undefined;
        }

        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(6)))
        {
            return MilestoneDueStatus.DueSoon;
        }

        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(-7)))
        {
            return MilestoneDueStatus.Due;
        }

        return MilestoneDueStatus.Overdue;
    }

    public MilestoneClaim WithAchievementDate(DateDetails? achievementDate, AhpProgramme programme, DateDetails? previousSubmissionDate)
    {
        var achievementDateTime = DateTimeExtensions.FromDateDetails(achievementDate);
        ValidateAchievementDate(achievementDate);
        ValidateDateIsNotFuture(achievementDateTime);
        ValidatePreviousSubmissionDate(previousSubmissionDate, achievementDateTime);
        ValidateProgrammeDates(programme, achievementDateTime!.Value);

        return new MilestoneClaim(Type, Status, GrantApportioned, ClaimDate, CostsIncurred, IsConfirmed);
    }

    public MilestoneClaim WithCostsIncurred(bool? costsIncurred)
    {
        if (costsIncurred.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                ValidationErrorMessage.MustBeSelectedYes(
                    "you have incurred costs and made payments to at least the level of the grant"));
        }

        return new MilestoneClaim(
            Type,
            Status,
            GrantApportioned,
            ClaimDate,
            costsIncurred,
            IsConfirmed);
    }

    public MilestoneClaim WithConfirmation(bool? isConfirmed)
    {
        if (isConfirmed != true)
        {
            OperationResult.ThrowValidationError(nameof(IsConfirmed), "Confirm the declaration to continue");
        }

        return new MilestoneClaim(
            Type,
            Status,
            GrantApportioned,
            ClaimDate,
            CostsIncurred,
            isConfirmed);
    }

    public CanceledMilestoneClaim Cancel()
    {
        if (Status != MilestoneStatus.Draft)
        {
            OperationResult.ThrowValidationError(nameof(MilestoneClaim), "Cannot cancel submitted claim");
        }

        return new CanceledMilestoneClaim(this);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Status;
        yield return GrantApportioned;
        yield return ClaimDate;
        yield return CostsIncurred;
        yield return IsConfirmed;
    }

    private void ValidateAchievementDate(DateDetails? achievementDate)
    {
        if (achievementDate.IsNotProvided() || achievementDate!.Day.IsNotProvided() || achievementDate.Month.IsNotProvided() ||
            achievementDate.Year.IsNotProvided())
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.MustProvideRequiredField("achievement date"));
        }
    }

    private void ValidateDateIsNotFuture(DateTime? achievementDateTime)
    {
        if (achievementDateTime!.Value.IsAfter(DateTime.Today))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), "The date must be today or in the past");
        }
    }

    private void ValidatePreviousSubmissionDate(DateDetails? previousSubmissionDate, DateTime? achievementDateTime)
    {
        var previousSubmissionDateTime = DateTimeExtensions.FromDateDetails(previousSubmissionDate);
        if (previousSubmissionDate.IsProvided() && achievementDateTime!.Value.IsBefore(previousSubmissionDateTime!.Value))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }
    }

    private void ValidateProgrammeDates(AhpProgramme programme, DateTime achievementDateTime)
    {
        var isStartOnSite = Type == MilestoneType.StartOnSite && programme.StartOnSiteDates.IsProvided();
        var isCompletion = Type == MilestoneType.Completion && programme.CompletionDates.IsProvided();
        var isAmountNonZero = GrantApportioned.Amount > 0;
        var isAmountZero = GrantApportioned.Amount == 0;

        if ((isStartOnSite && !DateWithinRange(achievementDateTime, programme.StartOnSiteDates)) ||
            (isCompletion && !DateWithinRange(achievementDateTime, programme.CompletionDates)) ||
            (isAmountNonZero && !DateWithinProgrammeAndFundingDates(achievementDateTime, programme)) ||
            (isAmountZero && !DateWithinProgrammeDates(achievementDateTime, programme)))
        {
            OperationResult.ThrowValidationError(nameof(ClaimDate.AchievementDate), ValidationErrorMessage.DatesOutsideTheProgramme);
        }
    }

    private bool DateWithinRange(DateTime date, DateRange range) => date.IsAfter(range.StartDate) && date.IsBeforeOrEqualTo(range.EndDate);

    private bool DateWithinProgrammeAndFundingDates(DateTime date, AhpProgramme programme) =>
        DateWithinRange(date, programme.ProgrammeDates) && DateWithinRange(date, programme.FundingDates);

    private bool DateWithinProgrammeDates(DateTime date, AhpProgramme programme) => DateWithinRange(date, programme.ProgrammeDates);
}
