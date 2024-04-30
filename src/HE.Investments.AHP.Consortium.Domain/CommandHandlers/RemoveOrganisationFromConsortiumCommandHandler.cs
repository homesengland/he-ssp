using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;

namespace HE.Investments.AHP.Consortium.Domain.CommandHandlers;

public class RemoveOrganisationFromConsortiumCommandHandler : ConsortiumCommandHandlerBase<RemoveOrganisationFromConsortiumCommand>
{
    public RemoveOrganisationFromConsortiumCommandHandler(IConsortiumRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task Perform(ConsortiumEntity consortium, RemoveOrganisationFromConsortiumCommand request, CancellationToken cancellationToken)
    {
        consortium.RemoveMember(request.OrganisationId, request.IsConfirmed);
        return Task.CompletedTask;
    }
}
