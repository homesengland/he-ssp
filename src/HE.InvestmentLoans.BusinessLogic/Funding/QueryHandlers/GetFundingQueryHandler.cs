using HE.InvestmentLoans.BusinessLogic.Funding.Mappers;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.Contract.Funding.Queries;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Funding.QueryHandlers;

public class GetFundingQueryHandler : IRequestHandler<GetFundingQuery, GetFundingQueryResponse>
{
    private readonly IAccountUserContext _loanUserContext;

    private readonly IFundingRepository _fundingRepository;

    public GetFundingQueryHandler(IAccountUserContext loanUserContext, IFundingRepository fundingRepository)
    {
        _loanUserContext = loanUserContext;
        _fundingRepository = fundingRepository;
    }

    public async Task<GetFundingQueryResponse> Handle(GetFundingQuery request, CancellationToken cancellationToken)
    {
        var funding = await _fundingRepository.GetAsync(
                                        request.LoanApplicationId,
                                        await _loanUserContext.GetSelectedAccount(),
                                        request.FundingFieldsSet,
                                        cancellationToken);

        return new GetFundingQueryResponse(FundingViewModelMapper.Map(funding));
    }
}
