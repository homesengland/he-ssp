using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public abstract class SaveHomeTypeSegmentCommandHandlerBase<TCommand> : HomeTypeCommandHandlerBase, IRequestHandler<TCommand, OperationResult>
    where TCommand : SaveHomeTypeSegmentCommandBase
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IAccountUserContext _accountUserContext;

    protected SaveHomeTypeSegmentCommandHandlerBase(IHomeTypeRepository homeTypeRepository, IAccountUserContext accountUserContext, ILogger logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _accountUserContext = accountUserContext;
    }

    protected abstract IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes { get; }

    protected abstract IEnumerable<Action<TCommand, IHomeTypeEntity>> SaveActions { get; }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            account,
            SegmentTypes,
            cancellationToken);

        var errors = PerformWithValidation(SaveActions.Select<Action<TCommand, IHomeTypeEntity>, Action>(x => () => x(request, homeType)).ToArray());
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(homeType, account.SelectedOrganisationId(), SegmentTypes, cancellationToken);
        return OperationResult.Success();
    }
}
