extern alias Org;

using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Account.Shared;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public sealed class ProvideDevelopingPartnerCommandHandler : UpdateSchemeCommandHandler<ProvideDevelopingPartnerCommand>
{
    public ProvideDevelopingPartnerCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, includeFiles: false)
    {
    }

    protected override void Update(SchemeEntity scheme, ProvideDevelopingPartnerCommand request)
    {
        var applicationPartners = scheme.ApplicationPartners.WithDevelopingPartner(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsPartnerConfirmed);
        scheme.ProvideApplicationPartners(applicationPartners);
    }
}
