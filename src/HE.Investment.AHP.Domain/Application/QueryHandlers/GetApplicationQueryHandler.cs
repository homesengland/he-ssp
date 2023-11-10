using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Common;
using MediatR;
using ContractApplication = HE.Investment.AHP.Contract.Application.Application;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, ContractApplication>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContractApplication> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await _repository.GetById(new(request.ApplicationId), cancellationToken);

        return new ContractApplication(
            application.Id.Value,
            application.Name.Name,
            EnumMapper.MapByName<Tenure, Contract.Application.Tenure>(application.Tenure?.Value));
    }
}
