using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class CompleteHomeTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<CompleteHomeTypeCommand, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IAccountUserContext _accountUserContext;

    public CompleteHomeTypeCommandHandler(IHomeTypeRepository homeTypeRepository, IAccountUserContext accountUserContext, ILogger<CompleteHomeTypeCommandHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(CompleteHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeType = await _homeTypeRepository.GetById(
            request.ApplicationId,
            request.HomeTypeId,
            account,
            HomeTypeSegmentTypes.All,
            cancellationToken);

        var errors = PerformWithValidation(() => homeType.CompleteHomeType(request.IsSectionCompleted));
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(homeType, account.SelectedOrganisationId(), HomeTypeSegmentTypes.None, cancellationToken);
        return OperationResult.Success();
    }
}
