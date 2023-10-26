using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class UpdateSchemeTenureCommandHandler  : UpdateSchemeCommandHandler<UpdateSchemeTenureCommand>
{
    public UpdateSchemeTenureCommandHandler(ISchemeRepository repository, IDomainExceptionHandler domainExceptionHandler)
        : base(repository, domainExceptionHandler)
    {
    }

    protected override void Update(UpdateSchemeTenureCommand request, SchemeEntity scheme)
    {
        scheme.ChangeTenure(new SchemeTenure(request.Tenure));
    }
}
