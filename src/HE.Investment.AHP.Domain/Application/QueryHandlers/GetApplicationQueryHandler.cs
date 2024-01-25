using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using MediatR;
using ContractApplication = HE.Investment.AHP.Contract.Application.Application;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, ContractApplication>
{
    private readonly IApplicationRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetApplicationQueryHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ContractApplication> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _repository.GetById(request.ApplicationId, account, cancellationToken);

        return new ContractApplication(
            application.Id,
            application.Name.Name,
            application.Tenure?.Value ?? default,
            application.Status,
            application.ReferenceNumber.Value,
            application.LastModified != null ? new ModificationDetails(application.LastModified.FirstName, application.LastModified.LastName, application.LastModified.ChangedOn) : null,
            application.Sections.Sections.Select(s => new ApplicationSection(s.Type, s.Status)).ToList());
    }
}
