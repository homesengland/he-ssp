using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Queries;

public record GetFinancialDetailsQuery(string ApplicationId) : IRequest<FinancialDetails>;
