using HE.Investments.Programme.Contract.Queries;
using HE.Investments.Programme.Domain.Mappers;
using HE.Investments.Programme.Domain.Repositories;
using MediatR;

namespace HE.Investments.Programme.Domain.QueryHandlers;

public class GetProgrammeQueryHandler : IRequestHandler<GetProgrammeQuery, Contract.Programme>
{
    private readonly IProgrammeRepository _repository;

    private readonly IProgrammeMapper _programmeMapper;

    public GetProgrammeQueryHandler(IProgrammeRepository repository, IProgrammeMapper programmeMapper)
    {
        _repository = repository;
        _programmeMapper = programmeMapper;
    }

    public async Task<Contract.Programme> Handle(GetProgrammeQuery request, CancellationToken cancellationToken)
    {
        var programme = await _repository.GetProgramme(request.ProgrammeId, cancellationToken);

        return _programmeMapper.Map(programme);
    }
}
