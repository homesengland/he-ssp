using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<ApplicationId?>>
{
    private readonly IApplicationRepository _repository;

    public CreateApplicationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<ApplicationId?>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _repository.Create(new ApplicationName(request.Name), cancellationToken);

        return new OperationResult<ApplicationId?>(application.Id);
    }
}
