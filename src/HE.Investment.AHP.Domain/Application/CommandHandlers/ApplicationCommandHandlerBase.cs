using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public abstract class ApplicationCommandHandlerBase
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly IApplicationRepository _applicationRepository;

    protected ApplicationCommandHandlerBase(
        IApplicationRepository applicationRepository,
        IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
    }

    protected async Task<OperationResult> Perform(Func<ApplicationEntity, Task> action, AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(applicationId, account, cancellationToken);

        await action(application);

        await _applicationRepository.Save(application, account.SelectedOrganisationId(), cancellationToken);
        await _applicationRepository.DispatchEvents(application, cancellationToken);

        return OperationResult.Success();
    }
}
