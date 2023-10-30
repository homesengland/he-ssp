using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
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
        var application = new ApplicationEntity(new ApplicationId(Guid.NewGuid().ToString()), new ApplicationName(request.Name));

        await _repository.Save(application, cancellationToken);

        return new OperationResult<ApplicationId?>(application.Id);
    }
}
