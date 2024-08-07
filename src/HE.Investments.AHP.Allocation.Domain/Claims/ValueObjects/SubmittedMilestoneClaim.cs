using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public sealed class SubmittedMilestoneClaim : MilestoneClaimBase
{
    public SubmittedMilestoneClaim(
        MilestoneType type,
        MilestoneStatus status,
        GrantApportioned grantApportioned,
        ClaimDate claimDate,
        bool? costsIncurred,
        bool? isConfirmed)
        : base(type, grantApportioned, claimDate, costsIncurred, isConfirmed)
    {
        if (status.IsNotIn(MilestoneStatus.Submitted, MilestoneStatus.UnderReview, MilestoneStatus.Approved, MilestoneStatus.Rejected, MilestoneStatus.Paid))
        {
            OperationResult.ThrowValidationError(nameof(status), "Invalid status for Submitted Claim");
        }

        Status = status;
    }

    public override bool IsSubmitted => true;

    public override bool IsEditable => false;

    public override MilestoneStatus Status { get; }

    public override MilestoneClaimBase WithAchievementDate(
        AchievementDate achievementDate,
        Programme.Contract.Programme programme,
        DateTime? previousSubmissionDate,
        DateTime currentDate)
    {
        throw new DomainValidationException(OperationNotAllowedResult("Providing achievement date"));
    }

    public override MilestoneClaimBase WithCostsIncurred(bool? costsIncurred)
    {
        throw new DomainValidationException(OperationNotAllowedResult("Providing costs incurred"));
    }

    public override MilestoneClaimBase WithConfirmation(bool? isConfirmed)
    {
        throw new DomainValidationException(OperationNotAllowedResult("Providing confirmation"));
    }

    public override MilestoneWithoutClaim Cancel()
    {
        throw new DomainValidationException(OperationNotAllowedResult("Cancellation"));
    }

    public override SubmittedMilestoneClaim Submit(DateTime currentDate)
    {
        throw new DomainValidationException(OperationNotAllowedResult("Submission"));
    }

    private static OperationResult OperationNotAllowedResult(string operationName)
    {
        return new OperationResult([new ErrorItem(nameof(SubmittedMilestoneClaim), $"{operationName} is not allowed for Submitted Claim")]);
    }
}
