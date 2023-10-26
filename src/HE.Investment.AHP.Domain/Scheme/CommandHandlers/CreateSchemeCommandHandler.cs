using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public class CreateSchemeCommandHandler : IRequestHandler<CreateSchemeCommand, OperationResult<SchemeId?>>
{
    private readonly ISchemeRepository _repository;
    private readonly IDomainExceptionHandler _domainExceptionHandler;

    public CreateSchemeCommandHandler(ISchemeRepository repository, IDomainExceptionHandler domainExceptionHandler)
    {
        _repository = repository;
        _domainExceptionHandler = domainExceptionHandler;
    }

    public async Task<OperationResult<SchemeId?>> Handle(CreateSchemeCommand request, CancellationToken cancellationToken)
    {
        return await _domainExceptionHandler.Handle(async () =>
        {
            var scheme = new SchemeEntity(new SchemeId(Guid.NewGuid().ToString()), new SchemeName(request.Name));

            await _repository.Save(scheme, cancellationToken);

            return new OperationResult<SchemeId?>(scheme.Id);
        });
    }
}
