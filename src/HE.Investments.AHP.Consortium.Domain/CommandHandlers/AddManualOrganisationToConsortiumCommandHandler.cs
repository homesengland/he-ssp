using System.Diagnostics.CodeAnalysis;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Organisation.Contract.Commands;
using HE.Investments.Programme.Contract;
using MediatR;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class AddManualOrganisationToConsortiumCommandHandler : DraftConsortiumCommandHandlerBase<AddManualOrganisationToConsortiumCommand>
{
    private readonly IMediator _mediator;

    public AddManualOrganisationToConsortiumCommandHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IMediator mediator,
        IAccountUserContext accountUserContext)
        : base(repository, draftConsortiumRepository, accountUserContext)
    {
        _mediator = mediator;
    }

    protected override async Task Perform(IConsortiumEntity consortium, AddManualOrganisationToConsortiumCommand request, CancellationToken cancellationToken)
    {
        var organisationResult = await _mediator.Send(
            new CreateManualOrganisationCommand(
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.TownOrCity,
                request.County,
                request.Postcode),
            cancellationToken);

        organisationResult.CheckErrors();

        await consortium.AddMember(organisationResult.ReturnedData, new NotPartOfConsortium(), cancellationToken);
    }
}

[SuppressMessage("Maintainability Rules", "SA1400", Justification = "False positive, 'file' scoped classes cannot use accessibility modifiers")]
sealed file record NotPartOfConsortium : IIsPartOfConsortium
{
    public Task<bool> IsPartOfConsortiumForProgramme(ProgrammeId programmeId, OrganisationId organisationId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
