using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class RemoveOrganisationFromConsortiumCommandHandler : DraftConsortiumCommandHandlerBase<RemoveOrganisationFromConsortiumCommand>
{
    private readonly IConsortiumRepository _repository;

    public RemoveOrganisationFromConsortiumCommandHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext)
        : base(repository, draftConsortiumRepository, accountUserContext)
    {
        _repository = repository;
    }

    protected override async Task Perform(IConsortiumEntity consortium, RemoveOrganisationFromConsortiumCommand request, CancellationToken cancellationToken)
    {
        await consortium.RemoveMember(request.OrganisationId, request.IsConfirmed, _repository, cancellationToken);
    }
}
