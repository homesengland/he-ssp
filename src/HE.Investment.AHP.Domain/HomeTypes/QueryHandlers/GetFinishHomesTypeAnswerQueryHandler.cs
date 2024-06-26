using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public class GetFinishHomesTypeAnswerQueryHandler : IRequestHandler<GetFinishHomesTypeAnswerQuery, ApplicationHomeTypesFinishAnswer>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IConsortiumUserContext _accountUserContext;

    public GetFinishHomesTypeAnswerQueryHandler(IHomeTypeRepository repository, IConsortiumUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationHomeTypesFinishAnswer> Handle(GetFinishHomesTypeAnswerQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);

        return new ApplicationHomeTypesFinishAnswer(
            homeTypes.Application.Name.Value,
            homeTypes.Status == SectionStatus.Completed ? FinishHomeTypesAnswer.Yes : FinishHomeTypesAnswer.Undefined);
    }
}
