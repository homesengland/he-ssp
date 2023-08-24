using MediatR;

namespace HE.InvestmentLoans.Contract.User.Queries;

public record GetUserDetailsQuery : IRequest<GetUserDetailsResponse>;

public record GetUserDetailsResponse(UserDetailsViewModel ViewModel);
