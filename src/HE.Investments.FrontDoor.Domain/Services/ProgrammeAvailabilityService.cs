using HE.Investments.FrontDoor.Domain.Programme;

namespace HE.Investments.FrontDoor.Domain.Services;

public class ProgrammeAvailabilityService : IProgrammeAvailabilityService
{
    public bool IsStartDateValidForAnyProgramme(IEnumerable<ProgrammeDetails> programmes, DateOnly? expectedStartDate)
    {
        foreach (var programme in programmes)
        {
            var isEndDateValid = programme.EndOn.HasValue
                                 && expectedStartDate < programme.EndOn;
            var isStartOnSiteEndDateValid = programme.StartOnSiteEndDate.HasValue
                                            && expectedStartDate < programme.StartOnSiteEndDate;

            if (isEndDateValid && isStartOnSiteEndDateValid)
            {
                return true;
            }
        }

        return false;
    }
}
