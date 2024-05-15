using HE.Investments.FrontDoor.Domain.Programme;

namespace HE.Investments.FrontDoor.Domain.Services;

public interface IProgrammeAvailabilityService
{
    bool IsStartDateValidForAnyProgramme(IEnumerable<ProgrammeDetails> programmes, DateOnly? expectedStartDate);
}
