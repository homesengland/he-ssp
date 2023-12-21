using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class IsApplicationNameAvailableQueryHandler : IRequestHandler<IsApplicationNameAvailableQuery, OperationResult>
{
    private readonly IApplicationRepository _repository;

    public IsApplicationNameAvailableQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(IsApplicationNameAvailableQuery request, CancellationToken cancellationToken)
    {
        var name = new ApplicationName(request.ApplicationName);

        var operationResult = OperationResult.New();
        if (await _repository.IsExist(name, cancellationToken))
        {
            operationResult.AddValidationError(new("Name", "There is already an application with this name. Enter a different name"));
        }

        return operationResult;
    }
}
