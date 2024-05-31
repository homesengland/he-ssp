using HE.Investments.Common.Extensions;
using HE.Investments.Programme.Contract;
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

    public async Task<bool> IsStartDateValidForProgramme(ProgrammeId programmeId, DateOnly? expectedStartDate, CancellationToken cancellationToken)
    {
        var programme = await _mediator.Send(new GetProgrammeQuery(programmeId), cancellationToken);

        var isEndDateValid = expectedStartDate < programme.ProgrammeDates.End;
        var isStartOnSiteEndDateValid = programme.StartOnSiteDates.End.IsNotProvided() || expectedStartDate < programme.StartOnSiteDates.End;

        return isEndDateValid && isStartOnSiteEndDateValid;
    }
}
