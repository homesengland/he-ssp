using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public abstract class SaveHomeTypeSegmentCommandHandlerBase<TCommand> : HomeTypeCommandHandlerBase, IRequestHandler<TCommand, OperationResult>
    where TCommand : ISaveHomeTypeSegmentCommand
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    private readonly bool _loadFiles;

    protected SaveHomeTypeSegmentCommandHandlerBase(
        IHomeTypeRepository homeTypeRepository,
        IConsortiumUserContext accountUserContext,
        ILogger logger,
        bool loadFiles = false)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _accountUserContext = accountUserContext;
        _loadFiles = loadFiles;
    }

    protected abstract IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes { get; }

    protected abstract IEnumerable<Action<TCommand, IHomeTypeEntity>> SaveActions { get; }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            cancellationToken,
            _loadFiles);

        var errors = PerformWithValidation(SaveActions.Select<Action<TCommand, IHomeTypeEntity>, Action>(x => () => x(request, homeType)).ToArray());
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(homeType, account, cancellationToken);
        return OperationResult.Success();
    }
}
