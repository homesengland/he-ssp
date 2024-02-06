using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class ChangeApplicationStatusBaseCommandHandler
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;

    public ChangeApplicationStatusBaseCommandHandler(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
    }

    protected async Task<OperationResult> Perform(Func<IApplicationRepository, ApplicationEntity, OrganisationId, Task> action, AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationId = account.SelectedOrganisationId();
        var application = await _applicationRepository.GetById(applicationId, account, cancellationToken);

        await action(_applicationRepository, application, organisationId);

        return OperationResult.Success();
    }
}
