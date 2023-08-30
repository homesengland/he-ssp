using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureSectionCommandHandler :
    CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideCompanyPurposeCommand>,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand>
{
    public CompanyStructureSectionCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task Handle(ProvideCompanyPurposeCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.ProvideCompanyPurpose(request.CompanyPurpose), request.LoanApplicationId, cancellationToken);
    }

    public async Task Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        await Perform(
            x =>
            {
                x.ProvideMoreInformation(request.OrganisationMoreInformation);
                x.ProvideFileWithMoreInformation(request.OrganisationMoreInformationFile);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
