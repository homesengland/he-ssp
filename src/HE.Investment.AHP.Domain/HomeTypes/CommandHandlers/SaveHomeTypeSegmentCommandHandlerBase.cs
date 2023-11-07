using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public abstract class SaveHomeTypeSegmentCommandHandlerBase<TCommand> : HomeTypeCommandHandlerBase, IRequestHandler<TCommand, OperationResult>
    where TCommand : SaveHomeTypeSegmentCommandBase
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    protected SaveHomeTypeSegmentCommandHandlerBase(IHomeTypeRepository homeTypeRepository, ILogger logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    protected abstract IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes { get; }

    protected abstract IEnumerable<Action<TCommand, IHomeTypeEntity>> SaveActions { get; }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            new HomeTypeId(request.HomeTypeId),
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(SaveActions.Select<Action<TCommand, IHomeTypeEntity>, Action>(x => () => x(request, homeType)).ToArray());
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(request.ApplicationId, homeType, SegmentTypes, cancellationToken);
        return OperationResult.Success();
    }
}
