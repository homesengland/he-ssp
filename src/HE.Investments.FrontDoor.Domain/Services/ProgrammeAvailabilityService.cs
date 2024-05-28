using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Services;

public class ProgrammeAvailabilityService : IProgrammeAvailabilityService
{
    private readonly IMediator _mediator;

    public ProgrammeAvailabilityService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> IsStartDateValidForProgramme(ProgrammeType programmeType, DateOnly? expectedStartDate, CancellationToken cancellationToken)
    {
        var programmes = await _mediator.Send(new GetProgrammesQuery(programmeType), cancellationToken);
        foreach (var programme in programmes)
        {
            var isEndDateValid = expectedStartDate < programme.EndDate;
            var isStartOnSiteEndDateValid = expectedStartDate < programme.StartOnSiteEndDate;

            if (isEndDateValid && isStartOnSiteEndDateValid)
            {
                return true;
            }
        }

        return false;
    }
}
