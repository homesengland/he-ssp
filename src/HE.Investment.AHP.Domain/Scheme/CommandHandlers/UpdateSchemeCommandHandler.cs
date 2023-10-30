using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public abstract class UpdateSchemeCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult<SchemeId?>>
    where TRequest : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand
{
    private readonly ISchemeRepository _repository;

    protected UpdateSchemeCommandHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<SchemeId?>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var scheme = await _repository.GetById(new SchemeId(request.Id), cancellationToken);

        Update(request, scheme);

        await _repository.Save(scheme, cancellationToken);

        return new OperationResult<SchemeId?>(scheme.Id);
    }

    protected abstract void Update(TRequest request, SchemeEntity scheme);
}
