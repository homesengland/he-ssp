using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Queries;
public record SearchLocalAuthoritiesQuery(string Phrase, int StartPage, int PageSize) : IRequest<OperationResult<LocalAuthorities>>;
