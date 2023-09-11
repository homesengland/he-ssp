using MediatR;

namespace HE.InvestmentLoans.Contract.User.Queries;

public record GetUserAccountQuery : IRequest<GetUserAccountResponse>;

public record GetUserAccountResponse(string Email, string UserGlobalId, Guid? SelectedAccountId, IList<Guid> AccountIds, string? FirstName, string? LastName, string? TelephoneNumber);
