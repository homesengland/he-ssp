using HE.Investment.AHP.Contract.AhpProgramme;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.AhpProgramme;

public class GetTheAhpProgrammeQueryHandler : IRequestHandler<GetTheAhpProgrammeQuery, Programme>
{
    private readonly IMediator _mediator;

    private readonly IProgrammeSettings _programmeSettings;

    public GetTheAhpProgrammeQueryHandler(IMediator mediator, IProgrammeSettings programmeSettings)
    {
        _mediator = mediator;
        _programmeSettings = programmeSettings;
    }

    public Task<Programme> Handle(GetTheAhpProgrammeQuery request, CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(_programmeSettings.AhpProgrammeId)), cancellationToken);
    }
}
