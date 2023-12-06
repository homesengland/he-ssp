using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class CompleteHomeTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<CompleteHomeTypeCommand, OperationResult>
{
    private readonly IHomeTypeRepository _homeTypeRepository;

    public CompleteHomeTypeCommandHandler(IHomeTypeRepository homeTypeRepository, ILogger<CompleteHomeTypeCommandHandler> logger)
        : base(logger)
    {
        _homeTypeRepository = homeTypeRepository;
    }

    public async Task<OperationResult> Handle(CompleteHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeType = await _homeTypeRepository.GetById(
            applicationId,
            new HomeTypeId(request.HomeTypeId),
            HomeTypeSegmentTypes.All,
            cancellationToken);

        var errors = PerformWithValidation(() => homeType.CompleteHomeType(request.IsSectionCompleted));
        if (errors.Any())
        {
            return new OperationResult(errors);
        }

        await _homeTypeRepository.Save(homeType, HomeTypeSegmentTypes.None, cancellationToken);
        return OperationResult.Success();
    }
}
