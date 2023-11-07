using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public abstract class UpdateApplicationCommandHandler<TRequest> : IRequestHandler<TRequest, OperationResult<ApplicationId?>>
    where TRequest : IRequest<OperationResult<ApplicationId?>>, IUpdateApplicationCommand
{
    private readonly IApplicationRepository _repository;

    protected UpdateApplicationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<ApplicationId?>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetById(new ApplicationId(request.Id), cancellationToken);

        Update(request, application);

        await _repository.Save(application, cancellationToken);

        return new OperationResult<ApplicationId?>(application.Id);
    }

    protected abstract void Update(TRequest request, ApplicationEntity application);
}
