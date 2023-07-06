using MediatR;

namespace HE.InvestmentLoans.Contract.User;

public record GetUserDetailsQuery : IRequest<GetUserDetailsResponse>;

public record GetUserDetailsResponse(string Email, string UserGlobalId, Guid SelectedAccountId, IList<Guid> AccountIds);
