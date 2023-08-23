using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureSectionCommandHandler :
    IRequestHandler<CompanyStructureSectionCommand>,
    IRequestHandler<UnCompleteCompanyStructureSectionCommand>
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public CompanyStructureSectionCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(CompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.CompleteSection(), request.LoanApplicationId, cancellationToken);
    }

    public async Task Handle(UnCompleteCompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.UnCompleteSection(), request.LoanApplicationId, cancellationToken);
    }

    private async Task Perform(Action<CompanyStructureEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(loanApplicationId, userAccount, cancellationToken);

        action(companyStructure);

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
    }
}
