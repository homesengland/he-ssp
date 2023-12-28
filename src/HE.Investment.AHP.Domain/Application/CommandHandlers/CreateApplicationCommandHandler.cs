using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<ApplicationId>>
{
    private readonly IApplicationRepository _repository;
    private readonly IAccountUserContext _accountUserContext;

    public CreateApplicationCommandHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<ApplicationId>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        var account = await _accountUserContext.GetSelectedAccount();
        if (await _repository.IsExist(name, account.SelectedOrganisationId(), cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        var applicationToCreate = ApplicationEntity.New(name, new ApplicationTenure(request.Tenure));
        var application = await _repository.Save(applicationToCreate, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<ApplicationId>(application.Id);
    }
}
