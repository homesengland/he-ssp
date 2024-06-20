using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Utils;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CompleteDeliveryPhaseCommandHandler : UpdateDeliveryPhaseCommandHandler<CompleteDeliveryPhaseCommand>
{
    private readonly IMediator _mediator;

    private readonly IProgrammeSettings _programmeSettings;

    private readonly IDateTimeProvider _dateTimeProvider;

    public CompleteDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IMediator mediator,
        IConsortiumUserContext accountUserContext,
        IProgrammeSettings programmeSettings,
        IDateTimeProvider dateTimeProvider)
        : base(repository, accountUserContext)
    {
        _mediator = mediator;
        _programmeSettings = programmeSettings;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override async Task<OperationResult> Update(IDeliveryPhaseEntity entity, CompleteDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(_programmeSettings.AhpProgrammeId)), cancellationToken);

        entity.Complete(programme, request.IsSectionCompleted, _dateTimeProvider);

        return OperationResult.Success();
    }
}
