using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Entities;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Scheme.CommandHandlers;

public abstract class UpdateSchemeCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IUpdateSchemeCommand, IRequest<OperationResult>
{
    private readonly ISchemeRepository _repository;

    protected UpdateSchemeCommandHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var scheme = await _repository.GetById(applicationId, cancellationToken);

        Update(scheme!, request);

        await _repository.Save(applicationId, scheme!, cancellationToken);

        return new OperationResult<ApplicationId?>(applicationId);
    }

    protected abstract void Update(SchemeEntity scheme, TCommand request);
}
