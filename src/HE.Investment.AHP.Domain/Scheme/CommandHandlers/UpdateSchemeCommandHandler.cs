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
    private readonly bool _includeFiles;

    protected UpdateSchemeCommandHandler(ISchemeRepository repository, bool includeFiles)
    {
        _repository = repository;
        _includeFiles = includeFiles;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new ApplicationId(request.ApplicationId);
        var scheme = await _repository.GetByApplicationId(applicationId, _includeFiles, cancellationToken);

        Update(scheme, request);

        await _repository.Save(scheme, cancellationToken);

        return new OperationResult<ApplicationId?>(applicationId);
    }

    protected abstract void Update(SchemeEntity scheme, TCommand request);
}
