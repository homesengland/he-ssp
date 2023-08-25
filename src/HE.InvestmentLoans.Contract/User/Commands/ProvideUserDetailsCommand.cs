using MediatR;

namespace HE.InvestmentLoans.Contract.User.Commands;

public record ProvideUserDetailsCommand(
    string FirstName,
    string Surname,
    string JobTitle,
    string TelephoneNumber,
    string SecondaryTelephoneNumber,
    string IsTermsAndConditionsAccepted) : IRequest;
