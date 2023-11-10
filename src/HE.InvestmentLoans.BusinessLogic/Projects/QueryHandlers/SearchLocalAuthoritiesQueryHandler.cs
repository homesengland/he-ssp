using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.QueryHandlers;
public class SearchLocalAuthoritiesQueryHandler : IRequestHandler<SearchLocalAuthoritiesQuery, OperationResult<LocalAuthoritiesViewModel>>
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public SearchLocalAuthoritiesQueryHandler(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
    }

    public async Task<OperationResult<LocalAuthoritiesViewModel>> Handle(SearchLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        if (request.Phrase.IsNotProvided())
        {
            return OperationResult
                .New()
                .AddValidationError("LocalAuthorityName", ValidationErrorMessage.LocalAuthorityNameIsEmpty)
                .AsGeneric<LocalAuthoritiesViewModel>();
        }

        var (itemsFound, totalItems) = await _localAuthorityRepository.Search(request.Phrase, request.StartPage, request.PageSize, cancellationToken);

        var result = new LocalAuthoritiesViewModel
        {
            Items = itemsFound.Select(c => new LocalAuthorityViewModel { Id = c.Id, Name = c.Name }).ToList(),
            TotalItems = totalItems,
            Page = request.StartPage,
            PageSize = request.PageSize,
            Phrase = request.Phrase,
        };

        return OperationResult.Success(result);
    }
}
