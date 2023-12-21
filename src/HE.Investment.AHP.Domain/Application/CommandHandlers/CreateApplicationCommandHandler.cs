using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<ApplicationId>>
{
    private readonly IApplicationRepository _repository;

    public CreateApplicationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<ApplicationId>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        if (await _repository.IsExist(name, cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        var applicationToCreate = ApplicationEntity.New(name, new ApplicationTenure(request.Tenure));
        var application = await _repository.Save(applicationToCreate, cancellationToken);

        return new OperationResult<ApplicationId>(application.Id);
    }
}
