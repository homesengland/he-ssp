using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public sealed class ProvideOwnerOfTheLandCommandHandler : UpdateSchemeCommandHandler<ProvideOwnerOfTheLandCommand>
{
    public ProvideOwnerOfTheLandCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, includeFiles: false)
    {
    }

    protected override void Update(SchemeEntity scheme, ProvideOwnerOfTheLandCommand request)
    {
        var applicationPartners = scheme.ApplicationPartners.WithOwnerOfTheLand(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsPartnerConfirmed);
        scheme.ProvideApplicationPartners(applicationPartners);
    }
}
