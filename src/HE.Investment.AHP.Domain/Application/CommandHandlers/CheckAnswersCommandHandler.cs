using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.CommandHandlers;

public class CheckAnswersCommandHandler : IRequestHandler<CheckAnswersCommand, OperationResult>
{
    private readonly IConsortiumUserContext _accountUserContext;
    private readonly IApplicationRepository _applicationRepository;

    public CheckAnswersCommandHandler(
        IApplicationRepository applicationRepository,
        IConsortiumUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _applicationRepository = applicationRepository;
    }

    public async Task<OperationResult> Handle(CheckAnswersCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetById(request.Id, account, cancellationToken);

        application.AreAllSectionsCompleted();

        return OperationResult.Success();
    }
}
