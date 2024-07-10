using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Claims.QueryHandlers;

internal sealed class GetAllocationClaimsQueryHandler : IRequestHandler<GetAllocationClaimsQuery, AllocationDetails>
{
    private readonly IAllocationRepository _allocationRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IClaimsContractMapper _contractMapper;

    public GetAllocationClaimsQueryHandler(
        IAllocationRepository allocationRepository,
        IAccountUserContext accountUserContext,
        IClaimsContractMapper contractMapper)
    {
        _allocationRepository = allocationRepository;
        _accountUserContext = accountUserContext;
        _contractMapper = contractMapper;
    }

    public async Task<AllocationDetails> Handle(GetAllocationClaimsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var allocation = await _allocationRepository.GetById(request.AllocationId, userAccount, cancellationToken);

        return _contractMapper.Map(allocation, request.PaginationRequest);
    }
}
