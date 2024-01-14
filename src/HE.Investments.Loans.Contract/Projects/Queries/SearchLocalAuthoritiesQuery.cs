using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Queries;
public record SearchLocalAuthoritiesQuery(string Phrase, int StartPage, int PageSize) : IRequest<OperationResult<LocalAuthoritiesViewModel>>;
