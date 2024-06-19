using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public sealed class ProvideOwnerOfTheHomesCommandHandler : UpdateSchemeCommandHandler<ProvideOwnerOfTheHomesCommand>
{
    public ProvideOwnerOfTheHomesCommandHandler(ISchemeRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext, includeFiles: false)
    {
    }

    protected override void Update(SchemeEntity scheme, ProvideOwnerOfTheHomesCommand request)
    {
        var applicationPartners = scheme.ApplicationPartners.WithOwnerOfTheHomes(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsPartnerConfirmed);
        scheme.ProvideApplicationPartners(applicationPartners);
    }
}
