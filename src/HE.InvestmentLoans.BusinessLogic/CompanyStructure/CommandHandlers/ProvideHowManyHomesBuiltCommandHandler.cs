using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideHowManyHomesBuiltCommandHandler : IRequestHandler<ProvideHowManyHomesBuiltCommand>
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public ProvideHowManyHomesBuiltCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(ProvideHowManyHomesBuiltCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(request.LoanApplicationId, userAccount, cancellationToken);

        companyStructure.ProvideHowManyHomesBuilt(request.HomesBuilt);

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
    }
}
