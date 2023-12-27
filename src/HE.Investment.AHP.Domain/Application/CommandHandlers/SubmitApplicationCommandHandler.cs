using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class SubmitApplicationCommandHandler : UpdateApplicationCommandHandler<SubmitApplicationCommand>
{
    public SubmitApplicationCommandHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task Update(SubmitApplicationCommand request, ApplicationEntity application, CancellationToken cancellationToken)
    {
        application.Submit();
        return Task.CompletedTask;
    }
}
