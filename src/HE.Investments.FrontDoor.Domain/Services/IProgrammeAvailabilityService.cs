using HE.Investments.Programme.Contract.Enums;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IProgrammeAvailabilityService
{
    Task<bool> IsStartDateValidForProgramme(ProgrammeType programmeType, DateOnly? expectedStartDate, CancellationToken cancellationToken);
}
