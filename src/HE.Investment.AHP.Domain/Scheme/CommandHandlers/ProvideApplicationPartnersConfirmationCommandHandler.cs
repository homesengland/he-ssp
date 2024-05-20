using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public sealed class ProvideApplicationPartnersConfirmationCommandHandler : UpdateSchemeCommandHandler<ProvideApplicationPartnersConfirmationCommand>
{
    public ProvideApplicationPartnersConfirmationCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, includeFiles: false)
    {
    }

    protected override void Update(SchemeEntity scheme, ProvideApplicationPartnersConfirmationCommand request)
    {
        var applicationPartners = scheme.ApplicationPartners.WithAllPartnersConfirmation(request.AreAllPartnersConfirmed);
        scheme.ProvideApplicationPartners(applicationPartners);
    }
}
