using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Common.Queries;

public record GetConsortiumMembersQuery(PaginationRequest PaginationRequest) : IRequest<PaginationResult<ConsortiumMemberDetails>>;
