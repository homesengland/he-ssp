using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Factories;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, OperationResult<AhpApplicationId>>
{
    private readonly IApplicationRepository _repository;
    private readonly IAccountUserContext _accountUserContext;

    public CreateApplicationCommandHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult<AhpApplicationId>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.Name);
        var account = await _accountUserContext.GetSelectedAccount();
        if (await _repository.IsNameExist(name, account.SelectedOrganisationId(), cancellationToken))
        {
            throw new FoundException("Name", "There is already an application with this name. Enter a different name");
        }

        var applicationToCreate = ApplicationEntity.New(request.SiteId, name, new ApplicationTenure(request.Tenure), new ApplicationStateFactory(account));
        var application = await _repository.Save(applicationToCreate, account.SelectedOrganisationId(), cancellationToken);

        return new OperationResult<AhpApplicationId>(application.Id);
    }
}
