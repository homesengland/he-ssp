using HE.Investments.Programme.Contract.Queries;
using HE.Investments.Programme.Domain.Mappers;
using HE.Investments.Programme.Domain.Repositories;
using MediatR;

namespace HE.Investments.Programme.Domain.QueryHandlers;

public class GetProgrammesQueryHandler : IRequestHandler<GetProgrammesQuery, IList<Contract.Programme>>
{
    private readonly IProgrammeRepository _repository;

    private readonly IProgrammeMapper _programmeMapper;

    public GetProgrammesQueryHandler(IProgrammeRepository repository, IProgrammeMapper programmeMapper)
    {
        _repository = repository;
        _programmeMapper = programmeMapper;
    }

    public async Task<IList<Contract.Programme>> Handle(GetProgrammesQuery request, CancellationToken cancellationToken)
    {
        var programmes = await _repository.GetProgrammes(request.ProgrammeType, cancellationToken);

        return programmes.Select(_programmeMapper.Map).ToList();
    }
}
