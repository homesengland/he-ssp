using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public abstract class UpdateSchemeCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult<SchemeId?>>
    where TCommand : IUpdateSchemeCommand, IRequest<OperationResult<SchemeId?>>
{
    private readonly ISchemeRepository _repository;

    protected UpdateSchemeCommandHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<SchemeId?>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var scheme = await _repository.GetById(new SchemeId(request.SchemeId), cancellationToken);

        Update(scheme, request);

        await _repository.Save(scheme, cancellationToken);

        return new OperationResult<SchemeId?>(scheme.Id);
    }

    protected abstract void Update(SchemeEntity scheme, TCommand request);
}
