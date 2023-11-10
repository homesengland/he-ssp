using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class DuplicateHomeTypeCommandHandler : IRequestHandler<DuplicateHomeTypeCommand, OperationResult<HomeTypeId>>
{
    private readonly IHomeTypeRepository _repository;

    public DuplicateHomeTypeCommandHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<HomeTypeId>> Handle(DuplicateHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var applicationId = new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId);
        var homeTypes = await _repository.GetByApplicationId(applicationId, HomeTypeSegmentTypes.All, cancellationToken);
        var duplicatedHomeType = homeTypes.Duplicate(new HomeTypeId(request.HomeTypeId));

        await _repository.Save(applicationId, duplicatedHomeType, HomeTypeSegmentTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id!);
    }
}
