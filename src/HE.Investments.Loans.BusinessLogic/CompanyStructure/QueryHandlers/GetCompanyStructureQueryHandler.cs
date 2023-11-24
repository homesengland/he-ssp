using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Mappers;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;

public class GetCompanyStructureQueryHandler : IRequestHandler<GetCompanyStructureQuery, GetCompanyStructureQueryResponse>
{
    private readonly IAccountUserContext _loanUserContext;

    private readonly ICompanyStructureRepository _companyStructureRepository;

    public GetCompanyStructureQueryHandler(
        IAccountUserContext loanUserContext,
        ICompanyStructureRepository companyStructureRepository)
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
