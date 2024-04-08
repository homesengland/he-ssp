using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class IsApplicationExistQueryHandler : IRequestHandler<IsApplicationExistQuery, bool>
{
    private readonly IApplicationRepository _repository;
    private readonly IAccountUserContext _accountUserContext;

    public IsApplicationExistQueryHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<bool> Handle(IsApplicationExistQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        return await _repository.IsExist(request.ApplicationId, account.SelectedOrganisationId(), cancellationToken);
    }
}
