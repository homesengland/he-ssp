using MediatR;

namespace HE.InvestmentLoans.Contract.User.Commands;

public record ProvideUserDetailsCommand(
    string FirstName,
    string LastName,
    string JobTitle,
    string TelephoneNumber,
    string SecondaryTelephoneNumber) : IRequest;
