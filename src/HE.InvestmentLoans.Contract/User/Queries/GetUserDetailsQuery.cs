using MediatR;

namespace HE.InvestmentLoans.Contract.User.Queries;

public record GetUserDetailsQuery : IRequest<GetUserDetailsResponse>;

public record GetUserDetailsResponse(string FirstName, string Surname, string JobTitle, string TelephoneNumber, string SecondaryTelephoneNumber);
