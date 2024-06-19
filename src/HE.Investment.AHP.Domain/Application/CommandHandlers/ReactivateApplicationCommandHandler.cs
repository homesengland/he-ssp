using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class ReactivateApplicationCommandHandler : IRequestHandler<ReactivateApplicationCommand, OperationResult>
{
    private readonly IApplicationRepository _applicationRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    public ReactivateApplicationCommandHandler(IApplicationRepository applicationRepository, IConsortiumUserContext accountUserContext)
    {
        _applicationRepository = applicationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ReactivateApplicationCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        application.Reactivate();

        await _applicationRepository.Save(application, account, cancellationToken);
        await _applicationRepository.DispatchEvents(application, cancellationToken);

        return OperationResult.Success();
    }
}
