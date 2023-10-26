using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class UpdateSchemeNameCommandHandler : UpdateSchemeCommandHandler<UpdateSchemeNameCommand>
{
    public UpdateSchemeNameCommandHandler(ISchemeRepository repository, IDomainExceptionHandler domainExceptionHandler)
        : base(repository, domainExceptionHandler)
    {
    }

    protected override void Update(UpdateSchemeNameCommand request, SchemeEntity scheme)
    {
        scheme.ChangeName(new SchemeName(request.Name));
    }
}
