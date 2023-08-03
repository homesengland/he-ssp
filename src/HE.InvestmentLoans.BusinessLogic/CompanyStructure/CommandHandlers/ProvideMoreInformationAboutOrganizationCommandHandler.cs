using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandler : IRequestHandler<ProvideMoreInformationAboutOrganizationCommand>
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public ProvideMoreInformationAboutOrganizationCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(request.LoanApplicationId, userAccount, cancellationToken);

        companyStructure.ProvideMoreInformation(request.OrganisationMoreInformation);
        companyStructure.ProvideFileWithMoreInformation(request.OrganisationMoreInformationFile);

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
    }
}
