using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class RemoveOrganisationFromConsortiumCommandHandler : DraftConsortiumCommandHandlerBase<RemoveOrganisationFromConsortiumCommand>
{
    public RemoveOrganisationFromConsortiumCommandHandler(
        IConsortiumRepository repository,
        IDraftConsortiumRepository draftConsortiumRepository,
        IAccountUserContext accountUserContext)
        : base(repository, draftConsortiumRepository, accountUserContext)
    {
    }

    protected override Task Perform(IConsortiumEntity consortium, RemoveOrganisationFromConsortiumCommand request, CancellationToken cancellationToken)
    {
        consortium.RemoveMember(request.OrganisationId, request.IsConfirmed);
        return Task.CompletedTask;
    }
}
