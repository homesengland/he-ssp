using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public class MilestoneDatesInProgrammeDateRangePolicy : IMilestoneDatesInProgrammeDateRangePolicy
{
    private readonly IProgrammeRepository _programmeRepository;

    public MilestoneDatesInProgrammeDateRangePolicy(IProgrammeRepository programmeRepository)
    {
        _programmeRepository = programmeRepository;
    }

    public async Task Validate(DeliveryPhaseMilestones milestones, CancellationToken cancellationToken)
    {
        var programme = await _programmeRepository.GetCurrentProgramme(ProgrammeType.Ahp, cancellationToken);

        ValidateMilestone(milestones.AcquisitionMilestone, programme);
        ValidateMilestone(milestones.StartOnSiteMilestone, programme);
        ValidateMilestone(milestones.CompletionMilestone, programme);
    }

    private void ValidateMilestone<T>(MilestoneDetails<T>? milestone, ProgrammeBasicInfo programme)
        where T : DateValueObject
    {
        ValidateDateInProgrammeRange(milestone?.MilestoneDate, programme, "MilestoneStartAt", "Milestone date");
        ValidateDateInProgrammeRange(milestone?.PaymentDate, programme, "ClaimMilestonePaymentAt", "Milestone payment date");
    }

    private void ValidateDateInProgrammeRange(DateValueObject? date, ProgrammeBasicInfo programme, string fieldName, string fieldLabel)
    {
        if (date != null &&
            (date.Value < programme.StartAt || date.Value > programme.EndAt))
        {
            throw new DomainValidationException(fieldName, $"{fieldLabel} have to be within programme dates.");
        }
    }
}
