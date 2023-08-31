using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideCompanyPurposeCommandHandler : CompanyStructureBaseCommandHandler, IRequestHandler<ProvideCompanyPurposeCommand, OperationResult>
{
    public ProvideCompanyPurposeCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideCompanyPurposeCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var companyPurpose = request.CompanyPurpose.IsProvided() ? CompanyPurpose.FromString(request.CompanyPurpose!) : null;
                x.ProvideCompanyPurpose(companyPurpose);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
