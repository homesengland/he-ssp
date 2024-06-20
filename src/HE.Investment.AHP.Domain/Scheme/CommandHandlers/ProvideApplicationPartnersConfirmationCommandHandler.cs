using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public sealed class ProvideApplicationPartnersConfirmationCommandHandler : UpdateSchemeCommandHandler<ProvideApplicationPartnersConfirmationCommand>
{
    public ProvideApplicationPartnersConfirmationCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, includeFiles: false)
    {
    }

    protected override void Update(SchemeEntity scheme, ProvideApplicationPartnersConfirmationCommand request)
    {
        var applicationPartners = scheme.ApplicationPartners.WithPartnersConfirmation(request.ArePartnersConfirmed);
        scheme.ProvideApplicationPartners(applicationPartners);
    }
}
