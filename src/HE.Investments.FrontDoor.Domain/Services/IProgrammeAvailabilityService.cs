using HE.Investments.Programme.Contract;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IProgrammeAvailabilityService
{
    Task<bool> IsStartDateValidForProgramme(ProgrammeId programmeId, DateOnly? expectedStartDate, CancellationToken cancellationToken);
}
