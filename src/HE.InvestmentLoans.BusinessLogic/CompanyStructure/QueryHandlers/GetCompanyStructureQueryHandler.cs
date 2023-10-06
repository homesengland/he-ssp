using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Mappers;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureQueryHandler : IRequestHandler<GetCompanyStructureQuery, GetCompanyStructureQueryResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    public GetCompanyStructureQueryHandler(ILoanUserContext loanUserContext, ICompanyStructureRepository companyStructureRepository)
    {
        _loanUserContext = loanUserContext;
        _companyStructureRepository = companyStructureRepository;
    }

    public async Task<GetCompanyStructureQueryResponse> Handle(GetCompanyStructureQuery request, CancellationToken cancellationToken)
    {
        var companyStructure = await _companyStructureRepository.GetAsync(
                                        request.LoanApplicationId,
                                        await _loanUserContext.GetSelectedAccount(),
                                        request.CompanyStructureFieldsSet,
                                        cancellationToken);

        return new GetCompanyStructureQueryResponse(CompanyStructureViewModelMapper.Map(companyStructure));
    }
}
