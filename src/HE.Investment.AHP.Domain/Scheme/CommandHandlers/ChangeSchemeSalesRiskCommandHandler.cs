using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class ChangeSchemeSalesRiskCommandHandler : UpdateSchemeCommandHandler<ChangeSchemeSalesRiskCommand>
{
    public ChangeSchemeSalesRiskCommandHandler(ISchemeRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext, false)
    {
    }

    protected override void Update(SchemeEntity scheme, ChangeSchemeSalesRiskCommand request)
    {
        scheme.ChangeSalesRisk(new SalesRisk(request.SalesRisk));
    }
}
