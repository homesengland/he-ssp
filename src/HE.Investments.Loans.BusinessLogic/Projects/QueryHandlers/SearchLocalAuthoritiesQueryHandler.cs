extern alias Org;

using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Projects.Queries;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.Loans.BusinessLogic.Projects.QueryHandlers;

public class SearchLocalAuthoritiesQueryHandler : IRequestHandler<SearchLocalAuthoritiesQuery, OperationResult<LocalAuthoritiesViewModel>>
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public SearchLocalAuthoritiesQueryHandler(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
    }

    public async Task<OperationResult<LocalAuthoritiesViewModel>> Handle(SearchLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        var (itemsFound, totalItems) =
            await _localAuthorityRepository.Search(request.Phrase, request.StartPage, request.PageSize, cancellationToken);

        var result = new LocalAuthoritiesViewModel
        {
            Items = itemsFound.Select(c => new LocalAuthorityViewModel { Id = c.Code.ToString(), Name = c.Name }).ToList(),
            TotalItems = totalItems,
            Page = request.StartPage,
            PageSize = request.PageSize,
            Phrase = request.Phrase,
        };

        return OperationResult.Success(result);
    }
}
