using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class ProvideHowManyHomesBuiltCommandHandler :
    CompanyStructureBaseCommandHandler,
    IRequestHandler<ProvideHowManyHomesBuiltCommand, OperationResult>
{
    public ProvideHowManyHomesBuiltCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideHowManyHomesBuiltCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var homesBuild = request.HomesBuilt.IsProvided() ? HomesBuilt.FromString(request.HomesBuilt!) : null;
                x.ProvideHowManyHomesBuilt(homesBuild);
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
