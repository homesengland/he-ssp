using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class RemoveHomeTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<RemoveHomeTypeCommand, OperationResult>
{
    private readonly IHomeTypeRepository _repository;

    public RemoveHomeTypeCommandHandler(IHomeTypeRepository repository, ILogger<RemoveHomeTypeCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(RemoveHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), HomeTypeSegmentTypes.None, cancellationToken);
        var validationErrors = PerformWithValidation(() => homeTypes.Remove(new HomeTypeId(request.HomeTypeId), request.RemoveHomeTypeAnswer));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        await _repository.Save(homeTypes, cancellationToken);
        return OperationResult.Success();
    }
}
