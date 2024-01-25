using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class IsApplicationNameAvailableQueryHandler : IRequestHandler<IsApplicationNameAvailableQuery, OperationResult>
{
    private readonly IApplicationRepository _repository;
    private readonly IAccountUserContext _accountUserContext;

    public IsApplicationNameAvailableQueryHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(IsApplicationNameAvailableQuery request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.ApplicationName);
        var account = await _accountUserContext.GetSelectedAccount();
        var operationResult = OperationResult.New();

        if (await _repository.IsNameExist(name, account.SelectedOrganisationId(), cancellationToken))
        {
            operationResult.AddValidationError(new("Name", "There is already an application with this name. Enter a different name"));
        }

        return operationResult;
    }
}
