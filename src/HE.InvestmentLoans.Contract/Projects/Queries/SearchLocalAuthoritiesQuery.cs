using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Queries;
public record SearchLocalAuthoritiesQuery(LoanApplicationId LoanApplicationId, string Phrase, int StartPage, int PageSize) : IRequest<OperationResult<LocalAuthoritiesViewModel>>;
