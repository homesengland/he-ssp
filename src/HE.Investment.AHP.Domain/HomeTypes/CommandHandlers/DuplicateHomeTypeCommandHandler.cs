using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
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
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, HomeTypeSegmentTypes.All, cancellationToken);
        var duplicatedHomeType = homeTypes.Duplicate(new HomeTypeId(request.HomeTypeId));

        await _repository.Save(request.ApplicationId, duplicatedHomeType, HomeTypeSegmentTypes.All, cancellationToken);

        return OperationResult.Success(duplicatedHomeType.Id!);
    }
}
