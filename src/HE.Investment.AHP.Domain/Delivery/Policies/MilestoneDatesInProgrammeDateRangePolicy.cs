using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Delivery.Policies;

public class MilestoneDatesInProgrammeDateRangePolicy : IMilestoneDatesInProgrammeDateRangePolicy
{
    private readonly IAhpProgrammeRepository _programmeRepository;

    public MilestoneDatesInProgrammeDateRangePolicy(IAhpProgrammeRepository programmeRepository)
    {
        _programmeRepository = programmeRepository;
    }

    public async Task Validate(AhpApplicationId applicationId, DeliveryPhaseMilestones milestones, CancellationToken cancellationToken)
    {
        var programme = await _programmeRepository.GetProgramme(applicationId, cancellationToken);

        ValidateMilestone(milestones.AcquisitionMilestone, programme);
        ValidateMilestone(milestones.StartOnSiteMilestone, programme);
        ValidateMilestone(milestones.CompletionMilestone, programme);
    }

    private void ValidateMilestone<T>(MilestoneDetails<T>? milestone, AhpProgramme programme)
        where T : DateValueObject
    {
        ValidateDateInProgrammeRange(milestone?.MilestoneDate, programme, "MilestoneStartAt", "The milestone date");
        ValidateDateInProgrammeRange(milestone?.PaymentDate, programme, "ClaimMilestonePaymentAt", "The milestone payment date");
    }

    private void ValidateDateInProgrammeRange(DateValueObject? date, AhpProgramme programme, string fieldName, string fieldLabel)
    {
        if (date != null &&
            (date.Value < programme.ProgrammeDates.ProgrammeStartDate || date.Value > programme.ProgrammeDates.ProgrammeEndDate))
        {
            throw new DomainValidationException(fieldName, $"{fieldLabel} must be within the programme date");
        }
    }
}
