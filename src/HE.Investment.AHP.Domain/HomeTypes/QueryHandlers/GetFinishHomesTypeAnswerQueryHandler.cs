using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Domain;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public class GetFinishHomesTypeAnswerQueryHandler : IRequestHandler<GetFinishHomesTypeAnswerQuery, FinishHomeTypesAnswer>
{
    private readonly IHomeTypeRepository _repository;

    public GetFinishHomesTypeAnswerQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinishHomeTypesAnswer> Handle(GetFinishHomesTypeAnswerQuery request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), HomeTypeSegmentTypes.None, cancellationToken);

        return homeTypes.Status == SectionStatus.Completed ? FinishHomeTypesAnswer.Yes : FinishHomeTypesAnswer.Undefined;
    }
}
