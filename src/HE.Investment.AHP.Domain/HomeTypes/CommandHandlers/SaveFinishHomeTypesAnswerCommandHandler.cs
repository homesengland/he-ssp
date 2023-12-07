using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveFinishHomeTypesAnswerCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<SaveFinishHomeTypesAnswerCommand, OperationResult>
{
    private readonly IHomeTypeRepository _repository;

    public SaveFinishHomeTypesAnswerCommandHandler(IHomeTypeRepository repository, ILogger<SaveFinishHomeTypesAnswerCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(SaveFinishHomeTypesAnswerCommand request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), HomeTypeSegmentTypes.None, cancellationToken);
        var validationErrors = PerformWithValidation(() => homeTypes.CompleteSection(request.FinishHomeTypesAnswer));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        if (!request.IsCheckOnly)
        {
            await _repository.Save(homeTypes, cancellationToken);
        }

        return OperationResult.Success();
    }
}
