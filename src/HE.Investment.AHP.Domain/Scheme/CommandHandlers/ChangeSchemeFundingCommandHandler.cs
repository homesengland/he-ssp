using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Contract.Scheme.Events;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Infrastructure.Events;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeFundingCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeFundingCommand>
{
    private readonly IEventDispatcher _dispatcher;

    public ChangeSchemeFundingCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext, IEventDispatcher eventDispatcher)
        : base(repository, accountUserContext, false)
    {
        _dispatcher = eventDispatcher;
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeFundingCommand request)
    {
        var currentNumberOfHomes = scheme.Funding.HousesToDeliver;

        var funding = new SchemeFunding(request.RequiredFunding, request.HousesToDeliver);
        scheme.ChangeFunding(funding);

        if (currentNumberOfHomes != funding.HousesToDeliver)
        {
            _dispatcher.Publish(new SchemeNumberOfHomesHasBeenUpdatedEvent(scheme.Application.Id), CancellationToken.None);
        }
    }
}
