using MediatR;

namespace HE.InvestmentLoans.Contract.User.Commands;

public record ProvideUserDetailsCommand(UserDetailsViewModel UserDetailsViewModel) : IRequest;
