using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.QueryHandlers;

internal sealed class GetPhaseClaimsQueryHandler : IRequestHandler<GetPhaseClaimsQuery, Phase>
{
    private readonly IPhaseRepository _phaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IPhaseContractMapper _contractMapper;

    public GetPhaseClaimsQueryHandler(IPhaseRepository phaseRepository, IAccountUserContext accountUserContext, IPhaseContractMapper contractMapper)
    {
        _phaseRepository = phaseRepository;
        _accountUserContext = accountUserContext;
        _contractMapper = contractMapper;
    }

    public async Task<Phase> Handle(GetPhaseClaimsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var phase = await _phaseRepository.GetById(request.PhaseId, request.AllocationId, userAccount, cancellationToken);

        return _contractMapper.Map(phase);
    }
}
